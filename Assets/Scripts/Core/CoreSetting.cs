using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThrowOthello
{
    public class FieldSetting
    {
        //駒を整理するときに持ち上げる高さの係数
        public const float PieceMoveHeight = 1.0f;
        //フィールド一辺の長さ
        public const int FieldSize = 8;
        //フィールドの一辺当たりのマスの数
        public const int NumberOfSquares = 8;
        //一マスの一辺の長さ
        public const float LengthOfSquare = 1;

        //初期状態でおいてあるコマの数
        public const int NumberOfInitialPieces = 4;
        //プレイヤーが投げるコマの数
        public const int NumberOfPieces = 64;
    }

    public class AppSetting
    {
        //駒を動かすときのSmoothtime
        public const float PieceMoveSmoothTime = 0.1f;
        //駒の回転をリセットするときののSmoothtime
        public const float PieceRotateSmoothTime = 0.1f;
        //駒をひっくり返すスピード
        public const int PieceReverseSpeed = 4;

        public const float Epsilon = 0.1f;
    }
}