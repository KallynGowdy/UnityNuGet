using UnityEngine;
using System.Collections;

public class TestMatrix : MonoBehaviour
{
   public Vector3 rotation = new Vector3();
   public Vector3 scale = new Vector3(1,1,1);
   public Vector3 pos = new Vector3(1,1,1);

   public Vector3 rotation2 = new Vector3();
   public Vector3 scale2 = new Vector3(1, 1, 1);
   public Vector3 pos2 = new Vector3(0, 0, 0);

   public Rect fstBtnVal = new Rect(0, 0, 50, 50);

   public void Rotate(float value)
   {
       rotation.z = value;
   }

   public Vector3 fstPoint;

   public Vector3 point;

   Rect ToRect(Vector3 v, float width, float height)
   {
       return new Rect(v.x, v.y, width, height);
   }

   void OnGUI()
   {
      // Matrix4x4 before = GUI.matrix;
      // Matrix4x4 matrix = GUI.matrix;

       //fstPoint = matrix.MultiplyVector(new Vector3(fstBtnVal.xMin, fstBtnVal.yMin));

       //matrix.SetTRS(pos, Quaternion.Euler(rotation), scale);

       
       //point = matrix.inverse.MultiplyVector(new Vector3(fstBtnVal.xMin, fstBtnVal.yMin));
       //matrix.SetTRS(fstPoint - point, Quaternion.Euler(rotation), scale);
        //GUI.matrix = matrix;
       //GUI.Box(ToRect(point, 5, 5), "h");
       //GUI.Box(ToRect(fstPoint, 5, 5), "h");


       GUI.depth = 1;
       if (GUI.Button(fstBtnVal, "hi"))
       {
           Rotate(rotation.z + 10);
           print("hit");
               
       }
       //GUI.matrix.SetTRS(new Vector3(), Quaternion.Euler(0,0,0), new Vector3());
       //before.SetTRS(pos2, Quaternion.Euler(rotation2), scale2);
       //GUI.matrix = before;
       //GUI.Box(ToRect(point, 5, 5), "h");
       GUI.depth = 0;
       if (GUI.Button(new Rect(0, 50, 50, 50), "ho"))
       {
           rotation2.z += 20;
       }
   }

}
