using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMath
{
    /** 将用两个角度表示的方向转化为单位向量
     * phi：方向和 y 轴的夹角
     * theta：方向在 xz 平面的投影和 x 轴的夹角 */
    public static Vector3 DirAngleToVector(float phi, float theta) {
        float SinPhi = Mathf.Sin(phi);
        float CosPhi = Mathf.Cos(phi);
        float SinTheta = Mathf.Sin(theta);
        float CosTheta = Mathf.Cos(theta);
        return new Vector3(SinPhi*CosTheta, CosPhi, SinPhi*SinTheta);
    }

    /** 角度转弧度 */
    public static float DegreeToRadian(float degree) {
        return degree * Mathf.PI / 180f;
    }

    /** 高斯随机 
     * mean：均值
     * std_dev：标准差 */
    public static float RandomGaussian(float mean, float std_dev) {
        float u1 = (float)(1.0 - Random.Range(0f, 1f));
        float u2 = (float)(1.0 - Random.Range(0f, 1f));
        float rand_std_normal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + std_dev * rand_std_normal;
    }
}
