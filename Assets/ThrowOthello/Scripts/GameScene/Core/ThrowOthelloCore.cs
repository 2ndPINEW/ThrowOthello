using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThrowOthello.Core.Settings;
using ThrowOthello;
using ThrowOthello.Core.Network;

namespace ThrowOthello.Core
{
    public class ThrowOthelloCore : MonoBehaviour
    {

        [SerializeField]
        GameObject PieceObjectPrefab;

        Field field = new Field();

        public GameObject[] AllPieceGameObjects;
        public PieceObject[] AllPieceObjects;

        public PieceObject[] GeneratePieceObjects;

        public int CountScore(Color color)
        {
            int count = 0;
            for (int i = 0; i < AllPieceObjects.Length; i++)
            {
                if (AllPieceObjects[i].GetScoreBaseColor() == color)
                    count++;
            }
            return count;
        }


        public int NumberOfPieces()
        {
            return AllPieceGameObjects.Length;
        }


        public Field GetFieldData()
        {
            return field;
        }


        /// <summary>
        /// フィールドのデータを今の盤面の状態にリセットする
        /// </summary>
        public void fieldInitialize()
        {
            field.Initialize();
            for (int i = 0; i < AllPieceObjects.Length; i++)
            {
                field.SetPiece(AllPieceObjects[i]);
            }
        }


        /// <summary>
        /// 反転する駒の探索
        /// </summary>
        public bool OthelloCHK()
        {
            var hasOthello = false;
            for (int i = 0; i < GeneratePieceObjects.Length; i++)
            {
                if (field.OthelloCHK(GeneratePieceObjects[i]))
                    hasOthello = true;
            }
            GeneratePieceObjects = new PieceObject[0];
            return hasOthello;
        }


        public void ResetGame()
        {
            field = new Field();

            for (int i = 0; i < AllPieceGameObjects.Length; i++)
            {
                Destroy(AllPieceGameObjects[i]);
            }

            AllPieceGameObjects = new GameObject[0]; ;
            AllPieceObjects = new PieceObject[0];
            GeneratePieceObjects = new PieceObject[0];
        }


        public bool isAllPieceRedy()
        {
            for (int i = 0; i < AllPieceObjects.Length; i++)
            {
                if (AllPieceObjects[i] == null)
                {
                    return false;
                }
                if (!AllPieceObjects[i].isRedy()) return false;
            }
            return true;
        }


        public void GenerateRandomPiece(int number)
        {
            GeneratePieceObjects = new PieceObject[0];
            for (int i = 0; i < number; i++)
            {
                var pos = new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), UnityEngine.Random.Range(2, 10f), UnityEngine.Random.Range(-4.5f, 4.5f));
                var rot = Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)));
                var obj = Instantiate(PieceObjectPrefab, pos, rot);
                Array.Resize(ref AllPieceGameObjects, AllPieceGameObjects.Length + 1);
                Array.Resize(ref AllPieceObjects, AllPieceObjects.Length + 1);
                Array.Resize(ref GeneratePieceObjects, GeneratePieceObjects.Length + 1);
                AllPieceGameObjects[AllPieceGameObjects.Length - 1] = obj;
                AllPieceObjects[AllPieceObjects.Length - 1] = obj.GetComponent<PieceObject>();
                GeneratePieceObjects[i] = obj.GetComponent<PieceObject>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        /// <param name="isLastGeneratePiece">これがtrueだと生成したオセロの駒の配列をりせっとする。生成した駒を順番にオセロ判定するから基本trueでいい気がする</param>
        /// <returns></returns>
        public PieceObject GpecificationGeneratePiece(Vector3 pos, Color color, string playerId, bool isLastGeneratePiece)
        {
            if (isLastGeneratePiece) GeneratePieceObjects = new PieceObject[0];

            var rot = Quaternion.Euler(0, 0, 0);
            if (color == Color.black) rot = Quaternion.Euler(90, 0, 0);
            if (color == Color.white) rot = Quaternion.Euler(270, 0, 0);
            if (color == Color.gray) rot = Quaternion.Euler(0, 0, 0);

            var obj = Instantiate(PieceObjectPrefab, pos, rot);
            Array.Resize(ref AllPieceGameObjects, AllPieceGameObjects.Length + 1);
            Array.Resize(ref AllPieceObjects, AllPieceObjects.Length + 1);
            Array.Resize(ref GeneratePieceObjects, GeneratePieceObjects.Length + 1);
            AllPieceGameObjects[AllPieceGameObjects.Length - 1] = obj;
            AllPieceObjects[AllPieceObjects.Length - 1] = obj.GetComponent<PieceObject>();
            AllPieceObjects[AllPieceObjects.Length - 1].id = (AllPieceObjects.Length - 1).ToString();
            return GeneratePieceObjects[GeneratePieceObjects.Length - 1] = obj.GetComponent<PieceObject>();
        }


        public void GeneratePiece(MoveData movedata, string playerId, bool isLastGeneratePiece)
        {
            var pieceObject = GpecificationGeneratePiece(new Position(0, 0).ToVector3(100), Color.gray, playerId, isLastGeneratePiece);
            pieceObject.SetMoveData(movedata, false);
        }


        public void FieldOrgnize()
        {
            for (int i = 0; i < AllPieceObjects.Length; i++)
            {
                AllPieceObjects[i].StartOrganize(AllPieceObjects[i].transform.position.ToPosition().ToPositionIndex());
            }
        }

        PiecePositionAndRotationListObject recent_data = new PiecePositionAndRotationListObject();
        public string allPiecePositionAndRotation()
        {
            PiecePositionAndRotationListObject o = new PiecePositionAndRotationListObject();
            for(int i = 0; i < AllPieceGameObjects.Length; i++)
            {
                PiecePositionAndRotation tmp = new PiecePositionAndRotation();
                tmp.moveData = AllPieceObjects[i].GetMoveData();
                tmp.isKinematic = AllPieceObjects[i].systemOperating;
                tmp.id = i.ToString();
                o.PiecePositionAndRotations.Add(tmp);
            }
            o.totalPieces = AllPieceObjects.Length;
            o.hasAllPiece = true;
            return JsonUtility.ToJson(o);
        }

        public PiecePositionAndRotationListObject allPiecePositionAndRotationObject()
        {
            PiecePositionAndRotationListObject o = new PiecePositionAndRotationListObject();
            for (int i = 0; i < AllPieceGameObjects.Length; i++)
            {
                PiecePositionAndRotation tmp = new PiecePositionAndRotation();
                tmp.moveData = AllPieceObjects[i].GetMoveData();
                tmp.isKinematic = AllPieceObjects[i].systemOperating;
                tmp.id = i.ToString();
                o.PiecePositionAndRotations.Add(tmp);
            }
            o.hasAllPiece = true;
            o.totalPieces = AllPieceObjects.Length;
            return o;
        }

        public PiecePositionAndRotationListObject diffPiecePositionAndRotation()
        {
            PiecePositionAndRotationListObject o = new PiecePositionAndRotationListObject();
            for (int i = 0; i < AllPieceGameObjects.Length; i++)
            {
               
                if(!AllPieceObjects[i].isRedy())
                {
                    PiecePositionAndRotation tmp = new PiecePositionAndRotation();
                    tmp.moveData = AllPieceObjects[i].GetMoveData();
                    tmp.isKinematic = AllPieceObjects[i].systemOperating;
                    tmp.id = i.ToString();
                    o.PiecePositionAndRotations.Add(tmp);
                }
            }
            o.totalPieces = AllPieceObjects.Length;
            return o;
        }

        public void OverWriteAllPiecePositionAndRotation(string jsonData)
        {
            PiecePositionAndRotationListObject o = JsonUtility.FromJson<PiecePositionAndRotationListObject>(jsonData);
            if (AllPieceGameObjects.Length > o.totalPieces || o.hasAllPiece)
            {
                ResetGame();
            }
            foreach (PiecePositionAndRotation tmp in o.PiecePositionAndRotations)
            {
                if(AllPieceObjects.Length <= int.Parse(tmp.id))
                {
                    //駒を新規で生成する(OverWriteされる側は判定しないし全部true
                    GeneratePiece(tmp.moveData, "", true);
                }
                AllPieceObjects[int.Parse(tmp.id)].SetMoveData(tmp.moveData, tmp.isKinematic);
            }
        }

        public bool OverWriteAllPiecePositionAndRotation(PiecePositionAndRotationListObject piecePositionAndRotationObject)
        {
            try{
                if (AllPieceGameObjects.Length > piecePositionAndRotationObject.totalPieces)
                {
                    ResetGame();
                }
                foreach (PiecePositionAndRotation tmp in piecePositionAndRotationObject.PiecePositionAndRotations)
                {
                    if (AllPieceObjects.Length <= int.Parse(tmp.id))
                    {
                        //駒を新規で生成する(OverWriteされる側は判定しないし全部true
                        GeneratePiece(tmp.moveData, "", true);
                    }
                    AllPieceObjects[int.Parse(tmp.id)].SetMoveData(tmp.moveData, tmp.isKinematic);
                }
                return false;
            }
            catch{
                return true;
            }
        }
    }

    public class Field
    {
        public Square[] Squares = new Square[0];


        //全部緑のマスたちを生成
        public Field()
        {
            Squares = new Square[0];
            for (int i = 0; i < FieldSetting.NumberOfSquares * FieldSetting.NumberOfSquares; i++)
            {
                Array.Resize(ref Squares, Squares.Length + 1);
                Squares[i] = new Square();
            }
        }


        //マス目の初期化
        public void Initialize()
        {
            for (int i = 0; i < FieldSetting.NumberOfSquares * FieldSetting.NumberOfSquares; i++)
            {
                Squares[i] = new Square();
            }
        }


        public void OverwriteSquares(Square[] squares)
        {
            Squares = squares;
        }

        //指定したマスにコマを追加
        public void SetPiece(PieceObject pieceObject)
        {
            if (!pieceObject.transform.position.ToPosition().ToPositionIndex().isWithinRange()) return;
            this.Squares[pieceObject.transform.position.ToPosition().ToPositionIndex().ToIndex()].AddPiece(pieceObject);
        }


        public bool OthelloCHK(PieceObject pieceObject)
        {
            var hasOthello = false;
            for(int i =  0;i < 8; i++)
            {
                var pos = pieceObject.transform.position.ToPosition().ToPositionIndex();
                if (pos.isWithinRange())
                    if (chk(pos, pos, i, pieceObject.PieceColor()))
                        hasOthello = true;
            }
            return hasOthello;
        }


        //オセロ判定
        //開始座標, 座標, 方向, 色
        bool chk(PositionIndex startPosIndex, PositionIndex posIndex, int direction, Color color)
        {
            if (direction == (int)Position.Direction.UP)        posIndex = new PositionIndex(posIndex.x, posIndex.y + 1);
            if (direction == (int)Position.Direction.DOWN)      posIndex = new PositionIndex(posIndex.x, posIndex.y - 1);
            if (direction == (int)Position.Direction.LEFT)      posIndex = new PositionIndex(posIndex.x - 1, posIndex.y);
            if (direction == (int)Position.Direction.RIGHT)     posIndex = new PositionIndex(posIndex.x + 1, posIndex.y);
            if (direction == (int)Position.Direction.UPLEFT)    posIndex = new PositionIndex(posIndex.x - 1, posIndex.y + 1);
            if (direction == (int)Position.Direction.UPRIGHT)   posIndex = new PositionIndex(posIndex.x + 1, posIndex.y + 1);
            if (direction == (int)Position.Direction.DOWNLEFT)  posIndex = new PositionIndex(posIndex.x - 1, posIndex.y - 1);
            if (direction == (int)Position.Direction.DOWNRIGHT) posIndex = new PositionIndex(posIndex.x + 1, posIndex.y - 1);

            //フィールド範囲外まで行ったら return
            if (!posIndex.isWithinRange()) return false;
            //緑のマスに当たったら return
            if (Squares[posIndex.ToIndex()].color == Color.green) return false;

            if (this.Squares[posIndex.ToIndex()].color == color &&
                isNextSquare(startPosIndex, posIndex))
            {
                return false;
            }

                //色が一致して隣あっていなかったらreverse
            if (this.Squares[posIndex.ToIndex()].color == color &&
                !isNextSquare(startPosIndex, posIndex))
            {
                Debug.Log(string.Format("{0},{1} から {2},{3}まで反転", startPosIndex.x, startPosIndex.y, posIndex.x, posIndex.y));
                Reverse(startPosIndex, posIndex, direction, color);
                return true;
            }
            return chk(startPosIndex, posIndex, direction, color);
        }

        bool isNextSquare(PositionIndex startPosIndex, PositionIndex endPosIndex)
        {
            if (Mathf.Abs(startPosIndex.x - endPosIndex.x) <= 1 &&
                Mathf.Abs(startPosIndex.y - endPosIndex.y) <= 1)
                return true;
            else
                return false;
        }


        void Reverse(PositionIndex posIndex, PositionIndex endPosIndex, int direction, Color color)
        {
            if (direction == (int)Position.Direction.UP) posIndex = new PositionIndex(posIndex.x, posIndex.y + 1);
            if (direction == (int)Position.Direction.DOWN) posIndex = new PositionIndex(posIndex.x, posIndex.y - 1);
            if (direction == (int)Position.Direction.LEFT) posIndex = new PositionIndex(posIndex.x - 1, posIndex.y);
            if (direction == (int)Position.Direction.RIGHT) posIndex = new PositionIndex(posIndex.x + 1, posIndex.y);
            if (direction == (int)Position.Direction.UPLEFT) posIndex = new PositionIndex(posIndex.x - 1, posIndex.y + 1);
            if (direction == (int)Position.Direction.UPRIGHT) posIndex = new PositionIndex(posIndex.x + 1, posIndex.y + 1);
            if (direction == (int)Position.Direction.DOWNLEFT) posIndex = new PositionIndex(posIndex.x - 1, posIndex.y - 1);
            if (direction == (int)Position.Direction.DOWNRIGHT) posIndex = new PositionIndex(posIndex.x + 1, posIndex.y - 1);

            var pieceObjects = Squares[posIndex.ToIndex()].PieceObjects;
            pieceObjects[pieceObjects.Length-1].StartReverse(posIndex, color);

            if (posIndex.x == endPosIndex.x && posIndex.y == endPosIndex.y)
            {
                return;
            }

            Reverse(posIndex, endPosIndex, direction, color);
        }
    }



    public class Square
    {
        public Color color;
        public PieceObject[] PieceObjects;


        public Square()
        {
            this.color = Color.green;
            this.PieceObjects = new PieceObject[0];
        }


        public void AddPiece(PieceObject pieceObject)
        {
            Array.Resize(ref PieceObjects, PieceObjects.Length + 1);
            PieceObjects[PieceObjects.Length-1] = pieceObject;

            //駒を高さの高い順に並べてindex登録
            float[] Heights = new float[0];
            for(int i = 0; i<PieceObjects.Length; i++)
            {
                Array.Resize(ref Heights, Heights.Length + 1);
                Heights[i] = PieceObjects[i].GetHeight();
            }

            Array.Sort(Heights, PieceObjects);

            for (int i = 0; i < PieceObjects.Length; i++)
            {
                PieceObjects[i].PileIndex = i;
            }

            //一番上の駒の色をマスの色に登録
            this.color = PieceObjects[PieceObjects.Length - 1].PieceColor();
        }
    }
}