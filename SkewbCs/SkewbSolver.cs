using System;
using System.Collections;
using System.Collections.Generic;

namespace SkewbCs
{
    public class Moves: List<char>
    {
        public Moves(IEnumerable<char> collection) :
            base(collection)
        {
        }

        public Moves() :
            base()
        { }
    }

    public class State : List<byte>
    {
        public State(IEnumerable<byte> collection) :
            base(collection)
        {
            CalcHash();
        }

        public State() :
            base()
        { }

        public (Int64,Int64) Hash { get; private set; }

        public void CalcHash()
        {
            Int64 a = 0;
            for (var i = 0; i <= 14; ++i)
            {
                a = (a << 3) | this[i];
            }
            Int64 b = 0;
            for (var i = 15; i <= 29; ++i)
            {
                b = (b << 3) | this[i];
            }

            this.Hash = (a, b);
        }
    }

    public class SkewbSolver
    {
        private List<State>[] distOverview;

        private const byte cW = 0;
        private const byte cG = 1;
        private const byte cR = 2;
        private const byte cB = 3;
        private const byte cO = 4;
        private const byte cY = 5;

        private readonly char[] possMoves = new char[] { 'U', 'u', 'R', 'r', 'L', 'l', 'B', 'b' };
        private readonly State solved = new State(new byte[] {
            cW, cG, cR, cB, cO, cY, cW, cW, cW, cW,
            cG, cG, cG, cG, cR, cR, cR, cR, cB, cB,
            cB, cB, cO, cO, cO, cO, cY, cY, cY, cY });

        public void Dist()
        {
            var idx = 0;
            this.distOverview = new List<State>[12];

            void Debug()
            {
                Console.WriteLine($"{idx}");
                for (int kk = 0; kk < 12; ++kk)
                {
                    Console.WriteLine($"{kk} {this.distOverview[kk].Count}");
                }
            }

            for (int i = 0; i < 12; ++i)
            {
                this.distOverview[i] = new List<State>();
            }
            
            var visited = new HashSet<(Int64,Int64)>();

            this.distOverview[0].Add(new State(this.solved));
            for (int i = 0; i < 12; ++i)
            {
                foreach (var poss in this.distOverview[i])
                {
                    foreach (var move in this.possMoves)
                    {
                        var newpos = Perform(move, poss);
                        if (!visited.Contains(newpos.Hash))
                        {
                            visited.Add(newpos.Hash);
                            this.distOverview[i + 1].Add(newpos); // Could overflow
                        }
                        if (++idx % 1000000 == 0)
                        {
                            Debug();
                        }
                    }
                }
            }
            Debug();
        }

        private bool LayerCheck(State cpos)
        {
            if (cpos[0] == cW)
            {
                if (cpos[6] == cW && cpos[7] == cW && cpos[8] == cW && cpos[9] == cW && cpos[10] == cG)
                    return true;
            }
            if (cpos[1] == cG)
            {
                if (cpos[10] == cG && cpos[11] == cG && cpos[12] == cG && cpos[13] == cG && cpos[9] == cW)
                    return true;
            }
            if (cpos[2] == cR)
            {
                if (cpos[14] == cR && cpos[15] == cR && cpos[16] == cR && cpos[17] == cR && cpos[7] == cW)
                    return true;
            }
            if (cpos[3] == cB)
            {
                if (cpos[18] == cB && cpos[19] == cB && cpos[20] == cB && cpos[21] == cB && cpos[7] == cW)
                    return true;
            }
            if (cpos[4] == cO)
            {
                if (cpos[22] == cO && cpos[23] == cO && cpos[24] == cO && cpos[25] == cO && cpos[9] == cW)
                    return true;
            }
            if (cpos[5] == cY)
            {
                if (cpos[26] == cY && cpos[27] == cY && cpos[28] == cY && cpos[29] == cY && cpos[12] == cG)
                    return true;
            }

            return false;
        }

        public List<State>[] LayDist()
        {
            var idx = 0;
            var overview = new List<State>[12];

            void Debug()
            {
                Console.WriteLine($"{idx}");
                for (int kk = 0; kk < 12; ++kk)
                {
                    Console.WriteLine($"{kk} {overview[kk].Count}");
                }
            }

            for (int i = 0; i < 12; ++i)
            {
                overview[i] = new List<State>();
            }

            foreach (var positions in this.distOverview)
            {
                foreach (var state in positions)
                {
                    overview[LaySolve(state)].Add(state);
                    if (++idx % 5000 == 0)
                    {
                        Debug();
                    }
                }
            }

            return overview;
        }

        private int LaySolve(State cpos)
        {
            if (this.LayerCheck(cpos))
            {
                return 0;
            }

            //var idx = 0;
            var q = new Queue<(State, Moves, char)>();
            q.Enqueue((new State(cpos), new Moves(), 'k'));

            var visited = new HashSet<(Int64, Int64)>();

            while (q.Count > 0)
            {
                var (s, path, lastMove) = q.Dequeue();

                if (!visited.Contains(s.Hash))
                {
                    visited.Add(s.Hash);
                    foreach (var move in this.possMoves)
                    {
                        if (move != lastMove)
                        {
                            var pos = Perform(move, s);
                            var newPath = new Moves(path);
                            newPath.Add(move);
                            if (LayerCheck(pos))
                            {
                                return newPath.Count;
                            }

                            q.Enqueue((pos, newPath, move));

                            //if (++idx % 10000 == 0)
                            //{
                            //    Console.WriteLine($"{idx} {newPath.Count}");
                            //}
                        }
                    }
                }
            }

            throw new Exception("Reached endof function");
        }

        private State Perform(char move, State cpos)
        {
            var pos = new State(cpos);
            switch (move)
            {
                case 'R':
                    pos[2] = cpos[5];
                    pos[3] = cpos[2];
                    pos[5] = cpos[3];
                    pos[7] = cpos[12];
                    pos[18] = cpos[17];
                    pos[15] = cpos[27];
                    pos[12] = cpos[25];
                    pos[17] = cpos[29];
                    pos[27] = cpos[20];
                    pos[25] = cpos[7];
                    pos[29] = cpos[18];
                    pos[20] = cpos[15];
                    pos[21] = cpos[16];
                    pos[28] = cpos[21];
                    pos[16] = cpos[28];
                    break;
                case 'r':
                    pos[5] = cpos[2];
                    pos[2] = cpos[3];
                    pos[3] = cpos[5];
                    pos[12] = cpos[7];
                    pos[17] = cpos[18];
                    pos[27] = cpos[15];
                    pos[25] = cpos[12];
                    pos[29] = cpos[17];
                    pos[20] = cpos[27];
                    pos[7] = cpos[25];
                    pos[18] = cpos[29];
                    pos[15] = cpos[20];
                    pos[16] = cpos[21];
                    pos[21] = cpos[28];
                    pos[28] = cpos[16];
                    break;
                case 'L':
                    pos[1] = cpos[4];
                    pos[5] = cpos[1];
                    pos[4] = cpos[5];
                    pos[10] = cpos[25];
                    pos[9] = cpos[20];
                    pos[23] = cpos[29];
                    pos[12] = cpos[23];
                    pos[27] = cpos[10];
                    pos[17] = cpos[9];
                    pos[25] = cpos[27];
                    pos[29] = cpos[12];
                    pos[20] = cpos[17];
                    pos[13] = cpos[24];
                    pos[26] = cpos[13];
                    pos[24] = cpos[26];
                    break;
                case 'l':
                    pos[4] = cpos[1];
                    pos[1] = cpos[5];
                    pos[5] = cpos[4];
                    pos[25] = cpos[10];
                    pos[20] = cpos[9];
                    pos[29] = cpos[23];
                    pos[23] = cpos[12];
                    pos[10] = cpos[27];
                    pos[9] = cpos[17];
                    pos[27] = cpos[25];
                    pos[12] = cpos[29];
                    pos[17] = cpos[20];
                    pos[24] = cpos[13];
                    pos[13] = cpos[26];
                    pos[26] = cpos[24];
                    break;
                case 'U':
                    pos[4] = cpos[0];
                    pos[0] = cpos[3];
                    pos[3] = cpos[4];
                    pos[7] = cpos[20];
                    pos[18] = cpos[25];
                    pos[15] = cpos[29];
                    pos[10] = cpos[15];
                    pos[9] = cpos[18];
                    pos[23] = cpos[7];
                    pos[25] = cpos[9];
                    pos[29] = cpos[10];
                    pos[20] = cpos[23];
                    pos[6] = cpos[19];
                    pos[19] = cpos[22];
                    pos[22] = cpos[6];
                    break;
                case 'u':
                    pos[0] = cpos[4];
                    pos[3] = cpos[0];
                    pos[4] = cpos[3];
                    pos[20] = cpos[7];
                    pos[25] = cpos[18];
                    pos[29] = cpos[15];
                    pos[15] = cpos[10];
                    pos[18] = cpos[9];
                    pos[7] = cpos[23];
                    pos[9] = cpos[25];
                    pos[10] = cpos[29];
                    pos[23] = cpos[20];
                    pos[19] = cpos[6];
                    pos[22] = cpos[19];
                    pos[6] = cpos[22];
                    break;
                case 'B':
                    pos[5] = cpos[4];
                    pos[3] = cpos[5];
                    pos[4] = cpos[3];
                    pos[19] = cpos[28];
                    pos[22] = cpos[21];
                    pos[6] = cpos[16];
                    pos[16] = cpos[13];
                    pos[21] = cpos[26];
                    pos[28] = cpos[24];
                    pos[24] = cpos[19];
                    pos[13] = cpos[6];
                    pos[26] = cpos[22];
                    pos[20] = cpos[29];
                    pos[25] = cpos[20];
                    pos[29] = cpos[25];
                    break;
                case 'b':
                    pos[4] = cpos[5];
                    pos[5] = cpos[3];
                    pos[3] = cpos[4];
                    pos[28] = cpos[19];
                    pos[21] = cpos[22];
                    pos[16] = cpos[6];
                    pos[13] = cpos[16];
                    pos[26] = cpos[21];
                    pos[24] = cpos[28];
                    pos[19] = cpos[24];
                    pos[6] = cpos[13];
                    pos[22] = cpos[26];
                    pos[29] = cpos[20];
                    pos[20] = cpos[25];
                    pos[25] = cpos[29];
                    break;
            }

            pos.CalcHash();
            return pos;
        }
    }
}
