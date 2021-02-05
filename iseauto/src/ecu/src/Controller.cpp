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
#define testingrn

int ticksSinceTarget;
float encoderValue;
float desiredValue;
int rate;
int timeoutTicks;
float pid_consts[3];
int sgn(double v) {
    if (v < 0) return -1;
    if (v > 0) return 1;
    return 0;
}
void velocityReferenceReset(float &vel, float p_vel)
{
    if(sgn(vel)+sgn(p_vel)==0)
        vel=0;
    else vel=p_vel;
}

void encoderReceive(std_msgs::Float32ConstPtr msg)
{

    ticksSinceTarget = 0;
    encoderValue=msg->data;

}
void desiredReceive(std_msgs::Float32ConstPtr msg)
{

    ticksSinceTarget = 0;
    desiredValue=msg->data;

}



class MotorController {
public:
    float max_torque = 60;
    /**PID Class*/
    class PID
    {
    public:
        float pError;
        float iError;
        float dError;
        float p;
        float i;
        float d;
        float max_i;
        float old_pError;
        float deadband[2];
        float output;




        PID(float cp, float ci, float cd, float max_out, float min_value){
            deadband[0]=max_out;
            deadband[1]=min_value;
            p=cp;
            i=ci;
            d=cd;
        }
        void update(float cp, float ci, float cd){
            
            p=cp;
            i=ci;
            d=cd;

        }
        void loop(float *desired, float *real)
        {
            pError = *desired - *real;
            iError += pError;
            dError = (pError - old_pError);
            checkInput(desired);
            checkOverflow(iError,max_i);
            output=p * pError + i * iError + d * dError;
            checkOverflow(output,deadband[0]);
            checkDeadband(output,deadband[1]);
        }


        float getOutput()
        {
            return output;
        }
        ~PID()
        {

        }

    private:
        void checkOverflowInteg(float &integ, float max_iError)
        {
            if (fabs(integ)>max_iError)
                integ=sgn(integ)*max_iError;
        }
        void checkOverflow(float &out, float max_out)
        {
            if (fabs(out)>max_out)
                out=sgn(out)*max_out;
        }
        float checkInput(float *des)
        {
            if (*des==0)iError=0;
        }
        void checkDeadband(float &out,float min_value)
        {
            if (fabs(out)<min_value)
                out=0;
        }

    };





    MotorController() {
        
        motorPID=new PID(    0.018f,  0.012, 0.0000f,   max_torque,  0);
        motorPID->max_i=500;
        rate=60;
        timeoutTicks=2;
        ros::NodeHandle pnode("~");
        pnode.getParam("twist_to_motor_rate", rate);
        pnode.getParam("twist_to_motor_timeout_ticks", timeoutTicks);

        // initializing publishers/subscribers
        encoderReceiver = handler.subscribe<std_msgs::Float32>("/iseauto/feedback/rpm", 10, encoderReceive);
        desiredReceiver = handler.subscribe<std_msgs::Float32>("iseauto/desired", 10, desiredReceive);



        controlPublisher = handler.advertise<std_msgs::Float32>("iseauto/control/torque", 10);




        

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

private:
    
    PID* motorPID;
    ros::NodeHandle handler;
    ros::Subscriber encoderReceiver;
    ros::Subscriber desiredReceiver;
    ros::Publisher controlPublisher;




    

    void spinOnce()
    {
        ros::spinOnce();

        motorPID->loop(&desiredValue,&encoderValue);
        std_msgs::Float32 value;
        value.data = motorPID->getOutput();
        controlPublisher.publish(value);

        
        ticksSinceTarget += 1;
        
        ROS_INFO("Looping");


    }

};


//TODO: cosine phi!!!!
int main(int argc, char **argv) {
    ros::init(argc, argv, "iseauto");
    ROS_INFO("Started iseauto Motor node");
    

    try {

        MotorController baseController;
        
        
        baseController.spin();

    }
    catch (const ros::Exception) {
        return (1);
    }

    return 0;
}
