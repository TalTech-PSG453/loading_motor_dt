using UnityEngine;

namespace DigitalTwin.Utils {
    public enum NoiseType {
        Uniform, Gaussian
    }

    public static class Noise {
        
        public static float ScalarNoise(float noiseStrength, NoiseType type) {
            return NoiseValue(noiseStrength, type);
        }

        public static Vector3 Vector3Noise(float noiseStrength, NoiseType type) {
            return new Vector3(NoiseValue(noiseStrength, type), NoiseValue(noiseStrength, type), NoiseValue(noiseStrength, type));
        }

        public static Vector2 Vector2LengthDirectionNoise(Vector2 value, float angleNoiseStrength, float lengthNoiseStrength, NoiseType type) {
            float newLength = value.magnitude + NoiseValue(lengthNoiseStrength, type);
            Vector2 result;
            if(value.magnitude == 0) {
                result = new Vector2(newLength, 0);
            } else {
                float scale = newLength / value.magnitude;
                result = value * scale;
            }

            float angle = NoiseValue(angleNoiseStrength, type) * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);
            float newX = result.x * cos - result.y * sin;
            float newY = result.x * sin + result.y * cos;
            result.x = newX;
            result.y = newY;
            return result;
        }

        private static float NoiseValue(float noiseStrength, NoiseType type) {
            switch(type) {
                case NoiseType.Uniform:
                    return Random.Range(-noiseStrength, noiseStrength);
                case NoiseType.Gaussian:
                    // Algorithm from https://stackoverflow.com/questions/218060/random-gaussian-variables
                    float u1 = 1f - Random.value;
                    float u2 = 1f - Random.value;
                    float normalDistribution = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
                    return noiseStrength * normalDistribution;
                default:
                    return 0;
            }
        }
    }
}