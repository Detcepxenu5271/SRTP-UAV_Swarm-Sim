using System; // Mathf
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 数学相关函数
 * 描述：将一些项目中常用的数学方法集中到 MyMath 单例类中
 * 使用方法：MyMath.instance.要调用的函数() */
public class MyMath : MonoBehaviour
{
    public static MyMath instance = null; // 单例模式

    public const float PRECISION = 0.000001f; // 1e-6

    private void Awake() {
        instance = this;
    }

    /** 将用两个角度表示的速度转化为单位向量
     * phi：方向和 y 轴的夹角
     * theta：方向在 xz 平面的投影和 x 轴的夹角 */
    public Vector3 VelocityAngleToVector(float phi, float theta) {
        float SinPhi = Mathf.Sin(phi);
        float CosPhi = Mathf.Cos(phi);
        float SinTheta = Mathf.Sin(theta);
        float CosTheta = Mathf.Cos(theta);
        return new Vector3(SinPhi*CosTheta, CosPhi, SinPhi*SinTheta);
    }

    public float DegreeToRadian(float degree) {
        return degree * Mathf.PI / 180f;
    }
}
