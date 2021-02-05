/**
 * twist_to_motors - converts a twist message to motor commands.  Needed for navigation stack
 *
 * This code is based on a similar node from the differential_drive package
 * http://wiki.ros.org/differential_drive
 * https://github.com/jfstepha/differential-drive
 *
 */

#include <ros/ros.h>
#include <std_msgs/Float32.h>

#include <iostream>
#include <fstream>
#include <ctime>
#include <thread>


using namespace std;
int ticksSinceTarget;

int closestItemIDX(vector<float> array, float item)
{
    float delta=-1;
    int idx=-1;
    //cout<<array.size()<<endl;
    for (int i = 0;i<array.size();i++)
    {
        float it=array.at(i);
        if(i==0)
        {
            delta=abs(it-item);

            idx=i;
        }
        else if(delta>abs(it-item)) {

            delta = abs(it - item);
            idx=i;
        }
        //cout<<i<<endl;



    }
    return idx;
}
vector<float> rpm;
vector<vector<float>> torque;
vector<vector<float>> rmsEfficiency;
struct state{
    float rpm;
    float torque;
}current_state;

int rate;
int timeoutTicks;

void getTorque(std_msgs::Float32ConstPtr msg)
{

    ticksSinceTarget = 0;
    current_state.torque=msg->data;
    //cout<<"Torque: "<<current_state.torque<<endl;

}
void getRPM(std_msgs::Float32ConstPtr msg)
{

    ticksSinceTarget = 0;
    current_state.rpm=msg->data;
    //cout<<"RPM: "<<current_state.rpm<<endl;


}





class EfficiencyMapProcessor{
public:

    string line;
    EfficiencyMapProcessor(string filename)
    {
        std::ifstream file(filename);
        int line_c=0;
        int word_c=0;

        while (std::getline(file, line))
        {
            word_c=0;
            string word = "";
            vector<float> temp_tor;
            vector<float> temp_eff;
            for (auto x : line)
            {
                if (x == ',')
                {
                    if(line_c==0&&word_c%2==0)
                    {
                        //cout<<word<<endl;
                        rpm.push_back(stof(word));
                    } else if(line_c==0&&word_c%2!=0)
                    {

                    }
                    else
                    {
                        if(word_c%2==0)
                        {
                            temp_tor.push_back(stof(word));
                        }
                        else
                        {

                            temp_eff.push_back(stof(word));
                        }
                    }
                    word_c++;
                    word = "";
                }
                else
                {
                    word = word + x;
                }
            }
            if(line_c==0&&word_c%2==0)
            {
                //cout<<word<<endl;
                rpm.push_back(stof(word));
            } else if(line_c==0&&word_c%2!=0)
            {

            }
            else
            {
                if(word_c%2==0)
                {
                    temp_tor.push_back(stof(word));
                }
                else
                {

                    temp_eff.push_back(stof(word));
                }
                torque.push_back(temp_tor);
                rmsEfficiency.push_back(temp_eff);
                word_c++;
            }

            line_c++;


        }
        cout<<torque.size()<<endl;
        cout<<torque.size()<<endl;



        rate=60;
        timeoutTicks=2;
        ros::NodeHandle pnode("~");
        pnode.getParam("twist_to_motor_rate", rate);
        pnode.getParam("twist_to_motor_timeout_ticks", timeoutTicks);

        // initializing publishers/subscribers
        TorqueReceiver = handler.subscribe<std_msgs::Float32>("iseauto/control/torque", 10, getTorque);
        RPMReceiver = handler.subscribe<std_msgs::Float32>("iseauto/feedback/rpm", 10, getRPM);



        EfficiencyControl = handler.advertise<std_msgs::Float32>("iseauto/control/torque/efficiency", 10);

    }



    float getEfficiency(float c_rpm, float c_torque)
    {
        int idx_rpm = closestItemIDX(rpm,c_rpm);
        //cout<<"Index RPM: "<<idx_rpm<<endl;
        vector<float> torque_column;
        for(auto item : torque)
        {
            //cout<<"LIne: "<<item.size()<<endl;
            torque_column.push_back(item.at(idx_rpm));
        }
        int idx_torque=closestItemIDX(torque_column,c_torque);
        //cout<<idx_torque<<endl;
        return rmsEfficiency.at(idx_torque).at(idx_rpm);





    }

    void spin() {




        ros::Rate r(rate);
        ros::Rate idle(10);
        ros::Time then = ros::Time::now();
        ros::spinOnce();
        ROS_INFO("Initialized");
        // main control loop
        while (ros::ok())
        {


            spinOnce();
            ros::spinOnce();
            idle.sleep();
        }

        ROS_INFO("Quit");


    }

    void spinOnce()
    {
        ros::spinOnce();

        std_msgs::Float32 value;
        value.data = current_state.torque*(getEfficiency(current_state.rpm,current_state.torque)/100);
        EfficiencyControl.publish(value);



    }

private:
    ros::NodeHandle handler;
    ros::Subscriber TorqueReceiver;
    ros::Subscriber RPMReceiver;
    ros::Publisher EfficiencyControl;


};





//TODO: cosine phi!!!!
int main(int argc, char **argv) {
    ros::init(argc, argv, "EffMap");
    ROS_INFO("Started iseauto Motor node");
    /*rpm.push_back(300);
    rpm.push_back(600);
    rpm.push_back(900);
    rpm.push_back(1200);
    rpm.push_back(1500);
    rpm.push_back(1650);
    rpm.push_back(1800);*/


    try {

        EfficiencyMapProcessor EffMapper("PMSynRM.csv");

        EffMapper.spin();

    }
    catch (const ros::Exception) {
        return (1);
    }

    return 0;
}
