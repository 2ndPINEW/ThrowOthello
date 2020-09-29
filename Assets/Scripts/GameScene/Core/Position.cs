using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThrowOthello
{
    public class PositionIndex
    {
        public int x;
        public int y;

        public PositionIndex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    public class Position
    {
        public float x;
        public float y;

        public Position(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            UPLEFT,
            UPRIGHT,
            DOWNLEFT,
            DOWNRIGHT
        }
    }


    public class PieceTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public int index = 0;

        public PieceTransform(Vector3 position, Quaternion rotation, int index)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.index = index;
        }
    }


    public static class PositionExtensions
    {

        public static Position ToPosition(this PositionIndex positionIndex)
        {
            var ReferencePosition = (-FieldSetting.FieldSize / 2) + (FieldSetting.LengthOfSquare / 2);
            return new Position( ReferencePosition + (positionIndex.x * FieldSetting.LengthOfSquare),
                                 ReferencePosition + (positionIndex.y * FieldSetting.LengthOfSquare)
                               );
        }


        public static Position ToPosition(this Vector3 pos)
        {
            return new Position(pos.x, pos.z);
        }


        public static Vector3 ToVector3(this Position pos, float height)
        {
            return new Vector3(pos.x, height, pos.y);
        }


        public static Vector3 ToVector3(this PositionIndex posIndex, float height)
        {
            return posIndex.ToPosition().ToVector3(height);
        }


        public static string ToString(this PositionIndex posIndex)
        {
            return string.Format("{0}, {1}", posIndex.x, posIndex.y);
        }


        public static bool isWithinRange(this PositionIndex posIndex)
        {
            if (0 <= posIndex.x && posIndex.x < FieldSetting.NumberOfSquares &&
                0 <= posIndex.y && posIndex.y < FieldSetting.NumberOfSquares)
                return true;
            else
                return false;
        }


        public static PositionIndex ToPositionIndex(this Position pos)
        {
            var ReferencePosition = (-FieldSetting.FieldSize / 2);

            if( !(ReferencePosition <= pos.x && pos.x <= Mathf.Abs(ReferencePosition) &&
                ReferencePosition <= pos.y && pos.y <= Mathf.Abs(ReferencePosition)))
            {
                return new PositionIndex(-1, -1);
            }

            for (int x = 0; x < FieldSetting.NumberOfSquares;x++)
            {
                for (int y = 0; y < FieldSetting.NumberOfSquares; y++)
                {
                    //xとyからその範囲を計算, 範囲内ならそのインデックスを返す
                    float[] xRange = new float[2] { ReferencePosition + (x * FieldSetting.LengthOfSquare), ReferencePosition + ((x + 1) * FieldSetting.LengthOfSquare) };
                    float[] yRange = new float[2] { ReferencePosition + (y * FieldSetting.LengthOfSquare), ReferencePosition + ((y + 1) * FieldSetting.LengthOfSquare) };
                    if( xRange[0] <= pos.x && pos.x <= xRange[1] &&
                        yRange[0] <= pos.y && pos.y <= yRange[1] )
                    {
                        return new PositionIndex(x, y);
                    }
                }
            }

            return new PositionIndex(-1, -1);
        }


        public static int ToIndex(this PositionIndex posIndex)
        {
            return posIndex.x + (posIndex.y * FieldSetting.NumberOfSquares);
        }

    }
}
