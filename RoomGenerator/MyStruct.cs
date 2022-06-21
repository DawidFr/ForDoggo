using System;


namespace  Special{
    
    [Serializable]
    public class MyStruct{

        [Serializable]
        public struct Vector4Bool{
            public bool x, y, z, w;
            public Vector4Bool(bool x = false, bool y = false, bool z = false, bool w = false){
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;

            }
        }

    }
}


