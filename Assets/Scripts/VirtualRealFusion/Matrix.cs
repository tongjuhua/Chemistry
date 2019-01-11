using UnityEngine;

namespace Matrix {

    public class MatrixUtils {
        public static Matrix3X3 Screw(double x, double y, double z) {
            Matrix3X3 r = new Matrix3X3 {
                m11 = 0,
                m12 = -z,
                m13 = y,
                m21 = z,
                m22 = 0,
                m23 = -x,
                m31 = -y,
                m32 = x,
                m33 = 0
            };
            return r;
        }

        public static double Determinant(Matrix3X3 m) {
            double r = 0;
            r += m.m11 * m.m22 * m.m33;
            r += m.m12 * m.m23 * m.m31;
            r += m.m13 * m.m21 * m.m32;
            r -= m.m13 * m.m22 * m.m31;
            r -= m.m12 * m.m21 * m.m33;
            r -= m.m11 * m.m23 * m.m32;
            return r;
        }

        public static Matrix3X3 Adjoint(Matrix3X3 m) {
            Matrix3X3 r = new Matrix3X3 {
                m11 = m.m22 * m.m33 - m.m23 * m.m32,
                m12 = m.m13 * m.m32 - m.m12 * m.m33,
                m13 = m.m12 * m.m23 - m.m13 * m.m22,
                m21 = m.m23 * m.m31 - m.m21 * m.m33,
                m22 = m.m11 * m.m33 - m.m13 * m.m31,
                m23 = m.m13 * m.m21 - m.m11 * m.m23,
                m31 = m.m21 * m.m32 - m.m22 * m.m31,
                m32 = m.m12 * m.m31 - m.m11 * m.m32,
                m33 = m.m11 * m.m22 - m.m12 * m.m21
            };
            return r;
        }

        public static Matrix3X3 Inverse(Matrix3X3 m) {
            Matrix3X3 r = (1.0 / Determinant(m)) * Adjoint(m);
            return r;
        }

        public static Quaternion MatrixToQuaternion(Matrix3X3 m) {
            Vector3 forward;
            forward.x = (float)(m.m13);
            forward.y = (float)(m.m23);
            forward.z = (float)(m.m33);

            Vector3 upwards;
            upwards.x = (float)(m.m12);
            upwards.y = (float)(m.m22);
            upwards.z = (float)(m.m32);

            return Quaternion.LookRotation(forward, upwards);
        }
    }

    [System.Serializable]
    public struct Matrix3X3 {
        public double m11;
        public double m12;
        public double m13;
        public double m21;
        public double m22;
        public double m23;
        public double m31;
        public double m32;
        public double m33;

        public static Matrix3X3 I {
            get {
                Matrix3X3 r = new Matrix3X3 {
                    m11 = 1,
                    m12 = 0,
                    m13 = 0,
                    m21 = 0,
                    m22 = 1,
                    m23 = 0,
                    m31 = 0,
                    m32 = 0,
                    m33 = 1
                };
                return r;
            }
        }

        public static Matrix3X3 operator +(Matrix3X3 a, Matrix3X3 b) {
            Matrix3X3 r = new Matrix3X3 {
                m11 = a.m11 + b.m11,
                m12 = a.m12 + b.m12,
                m13 = a.m13 + b.m13,
                m21 = a.m21 + b.m21,
                m22 = a.m22 + b.m22,
                m23 = a.m23 + b.m23,
                m31 = a.m31 + b.m31,
                m32 = a.m32 + b.m32,
                m33 = a.m33 + b.m33
            };
            return r;
        }

        public static Matrix3X3 operator -(Matrix3X3 a) {
            Matrix3X3 r = new Matrix3X3 {
                m11 = -a.m11,
                m12 = -a.m12,
                m13 = -a.m13,
                m21 = -a.m21,
                m22 = -a.m22,
                m23 = -a.m23,
                m31 = -a.m31,
                m32 = -a.m32,
                m33 = -a.m33
            };
            return r;
        }

        public static Matrix3X3 operator -(Matrix3X3 a, Matrix3X3 b) {
            Matrix3X3 r = new Matrix3X3 {
                m11 = a.m11 - b.m11,
                m12 = a.m12 - b.m12,
                m13 = a.m13 - b.m13,
                m21 = a.m21 - b.m21,
                m22 = a.m22 - b.m22,
                m23 = a.m23 - b.m23,
                m31 = a.m31 - b.m31,
                m32 = a.m32 - b.m32,
                m33 = a.m33 - b.m33
            };
            return r;
        }

        public static Matrix3X3 operator *(Matrix3X3 a, Matrix3X3 b) {
            Matrix3X3 r = new Matrix3X3 {
                m11 = a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * 1.0 * b.m31,
                m12 = a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * 1.0 * b.m32,
                m13 = a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * 1.0 * b.m33,
                m21 = a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * 1.0 * b.m31,
                m22 = a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * 1.0 * b.m32,
                m23 = a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * 1.0 * b.m33,
                m31 = a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * 1.0 * b.m31,
                m32 = a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * 1.0 * b.m32,
                m33 = a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * 1.0 * b.m33
            };
            return r;
        }

        public static Vector3 operator *(Matrix3X3 a, Vector3 b) {
            Vector3 r = new Vector3 {
                x = (float)(a.m11 * b.x + a.m12 * b.y + a.m13 * b.z),
                y = (float)(a.m21 * b.x + a.m22 * b.y + a.m23 * b.z),
                z = (float)(a.m31 * b.x + a.m32 * b.y + a.m33 * b.z)
            };
            return r;
        }

        public static Matrix3X3 operator *(double a, Matrix3X3 b) {
            Matrix3X3 r = new Matrix3X3 {
                m11 = a * b.m11,
                m12 = a * b.m12,
                m13 = a * b.m13,
                m21 = a * b.m21,
                m22 = a * b.m22,
                m23 = a * b.m23,
                m31 = a * b.m31,
                m32 = a * b.m32,
                m33 = a * b.m33
            };
            return r;
        }

        public static Matrix3X3 operator *(Matrix3X3 a, double b) {
            Matrix3X3 r = new Matrix3X3 {
                m11 = a.m11 * b,
                m12 = a.m12 * b,
                m13 = a.m13 * b,
                m21 = a.m21 * b,
                m22 = a.m22 * b,
                m23 = a.m23 * b,
                m31 = a.m31 * b,
                m32 = a.m32 * b,
                m33 = a.m33 * b
            };
            return r;
        }
    }
}