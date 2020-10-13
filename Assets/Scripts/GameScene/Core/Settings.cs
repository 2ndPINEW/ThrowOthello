using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThrowOthello.Core.Settings
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

        //コマが立っているか判定するときの角度
        public const float Epsilon = 0.1f;

        //白陣営のカメラ
        public Vector3 CameraPositionWhite = new Vector3(-6.8f, 6.8f, 0f);
        public Vector3 CameraAngleWhite = new Vector3(0f, 90f, 0f);

        //黒陣営のカメラ
        public Vector3 CameraPositionBlack = new Vector3(6.8f, 6.8f, 0f);
        public Vector3 CameraAngleBlack = new Vector3(0f, -90f, 0f);

        //スコアボードのカメラ
        public Vector3 CameraPositionScore = new Vector3(0f, 4.28f, 0f);
        public Vector3 CameraAngleScore = new Vector3(0f, 180f, 0f);
    }
}