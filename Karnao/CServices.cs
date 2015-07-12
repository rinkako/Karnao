using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Karnao
{
    // SOP结果类
    public class Sres
    {
        public Sres(List<int> p = null, int f = 0, string e = "")
        {
            _pos = new List<int>();
            _exp = e;
            _flag = f;
            for (int i = 0; p != null && i < p.Count; i++) { _pos.Add(p[i]); }
        }
        public int size()
        {
            return _pos.Count;
        }
        public void set_flag(int f)
        {
            _flag = f;
        }
        public void set_exp(string s)
        {
            _exp = s;
        }
        public string _exp;
        public List<int> _pos;
        public int _flag;
    }

    // 四位二进制类
    public class Fbit
    {
        public Fbit(uint _a = 0, uint _b = 0, uint _c = 0, uint _d = 0, uint _x = 0)
        {
            this.set_state(_a, _b, _c, _d, _x);
        }
        public void set_state(uint _a, uint _b, uint _c, uint _d, uint _x)
        {
            _proxy[0] = _a;
            _proxy[1] = _b;
            _proxy[2] = _c;
            _proxy[3] = _d;
            _dec = _x;
        }
        public uint fdec()
        {
            return _dec;
        }
        public uint gbit(int _bit)
        {
            return _proxy[_bit];
        }
        private uint[] _proxy = new uint[4];
        private uint _dec = 0;
    }

    // 卡诺图类
    public class Karno
    {
        // 构造函数
        public Karno()
        {
            this.clear();
        }
        // 填充无关项
        public void clear()
        {
            for (uint i = 0; i < 4; i++)
            {
                for (uint j = 0; j < 4; j++)
                {
                    table[i, j] = 8;
                }
            }
        }
        // 逻辑化简
        public string simplize()
        {
            // 进入缓冲队列
            Queue<int> _tqueue = new Queue<int>();
            #region 排队
            {
                _tqueue.Enqueue(Convert.ToInt32(this.table[0, 0]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[0, 1]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[0, 3]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[0, 2]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[1, 0]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[1, 1]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[1, 3]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[1, 2]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[3, 0]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[3, 1]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[3, 3]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[3, 2]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[2, 0]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[2, 1]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[2, 3]));
                _tqueue.Enqueue(Convert.ToInt32(this.table[2, 2]));
            }
            #endregion
            // 用8421计数器放入数组a中
            int _tint = 0;
            for (uint fa = 0; fa < 2; fa++)
            {
                for (uint fb = 0; fb < 2; fb++)
                {
                    for (uint fc = 0; fc < 2; fc++)
                    {
                        for (uint fd = 0; fd < 2; fd++)
                        {
                            _tint = _tqueue.Dequeue();
                            if (_tint == 1)
                            {
                                set_astate(fa, fb, fc, fd);
                            }
                            else if (_tint == 8)
                            {
                                set_xstate(fa, fb, fc, fd);
                            }
                        }
                    }
                }
            }
            // 计算并返回
            return this.akemi();
        }

        //====================== 化简卡诺图用的私有函数 =========================//
        // 化简函数
        private string akemi()
        {
            _sop = new List<Sres>();
            string tres = "";
            if (all_16bit() == true)
            {
                return "1";
            }
            else
            {
                // 主处理
                Madoka(all_8bit(), ref _sop);
                Madoka(all_4bit(), ref _sop);
                Madoka(all_2bit(), ref _sop);
                tres += all_1bit();
                // 找所有结果看看是否有包含的，有的话，消去
                for (int i = 0; i < _sop.Count; i++)
                {
                    for (int j = i + 1; j < _sop.Count; j++)
                    {
                        // 被包含
                        if (ListContain(_sop[i]._pos, _sop[j]._pos) == true)
                        {
                            _sop[j].set_flag(0);
                        }
                        else if (ListContain(_sop[j]._pos, _sop[i]._pos) == true)
                        {
                            _sop[i].set_flag(0);
                        }
                    }
                }
                // 再消去一次重复项
                List<Sres> _fsop = new List<Sres>();
                Madoka(_sop, ref _fsop);
                // 将剩余的项目添加到输出队列
                for (int i = 0; i < _fsop.Count; i++)
                {
                    if (_fsop[i]._flag != 0)
                    {
                        tres += _fsop[i]._exp;
                    }
                    if (i != _fsop.Count - 1)
                    {
                        tres += " + ";
                    }
                }

            }
            if (tres == "")
            {
                tres = "0";
            }
            return tres;
        }
        // 消去冗余项
        private void Madoka(List<Sres> _fliter, ref List<Sres> _sop)
        {
            for (int self = 0; self < _fliter.Count; self++)
            {
                int include_num = 0;

                List<int> _fx = new List<int>();
                for (int ffx = 0; ffx < _fliter[self]._pos.Count; ffx++) { _fx.Add(_fliter[self]._pos[ffx]); }

                for (int iter = 0; iter < _fliter.Count; iter++)
                {
                    if (self == iter || _fliter[iter]._flag == 0)
                    {
                        continue;
                    }
                    if (include_num == _fliter[self]._pos.Count)
                    {
                        _fliter[self].set_flag(0);
                        break;
                    }
                    for (int x = 0; x < _fx.Count; x++)
                    {
                        if (_fx.Count == 0) break;
                        for (int y = 0; y < _fliter[iter]._pos.Count; y++)
                        {
                            if (_fx[x] == _fliter[iter]._pos[y])
                            {
                                include_num++;
                                _fx.RemoveAt(x);
                                x = 0;
                                if (_fx.Count == 0) break;
                            }
                        }

                    }
                }
                if (include_num == _fliter[self]._pos.Count)
                {
                    _fliter[self].set_flag(0);
                }
            }
            for (int fi = 0; fi < _fliter.Count; fi++)
            {
                if (_fliter[fi]._flag != 0)
                {
                    _sop.Add(_fliter[fi]);
                }
            }
        }
        // 包含判定
        private bool ListContain(List<int> _org, List<int> _dst)
        {
            if (_org.Count < _dst.Count)
            {
                return false;
            }
            for (int i = 0; i < _dst.Count; i++)
            {
                if (_org.Contains(_dst[i]) == false)
                {
                    return false;
                }
            }
            return true;

        }
        // 置1数
        private void set_astate(uint true1, uint true2, uint true3, uint true4)
        {
            a[8 * true1 + 4 * true2 + 2 * true3 + true4] = 1;
        }
        // 置X数
        private void set_xstate(uint true1, uint true2, uint true3, uint true4)
        {
            a[8 * true1 + 4 * true2 + 2 * true3 + true4] = 8;
        }
        // 十六框
        private bool all_16bit()
        {
            for (int i = 0; i < 16; i++)
            {
                if (a[i] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        // 八框
        private List<Sres> all_8bit()
        {
            List<Sres> myret = new List<Sres>();
            string tres = "";
            for (int bit = 0; bit <= 3; bit++)
            {
                for (int ture = 0; ture <= 1; ture++)
                {
                    tres = "";
                    Sres ttres = new Sres();
                    ttres = judge_8bit(bit, ture);
                    tres += bit_to_char(bit);
                    if (ture == 0)
                    {
                        tres += "'";
                    }
                    //tres += " + ";

                    ttres.set_exp(tres);
                    if (ttres._flag != 0)
                    {
                        myret.Add(ttres);
                    }
                }
            }
            return myret;
        }
        // 四框
        private List<Sres> all_4bit()
        {
            List<Sres> myret = new List<Sres>();
            string tres = "";
            for (int bit1 = 3; bit1 >= 0; bit1--)
            {
                for (int bit2 = 0; bit2 < bit1; bit2++)
                {
                    for (int true1 = 0; true1 <= 1; true1++)
                    {
                        for (int true2 = 0; true2 <= 1; true2++)
                        {
                            tres = "";
                            Sres ttres = new Sres();
                            ttres = judge_4bit(bit1, true1, bit2, true2);
                            //if (judge_4bit(bit1, true1, bit2, true2) != 0)
                            //{
                            tres += bit_to_char(bit1);
                            if (true1 == 0)
                            {
                                tres += "'";
                            }
                            tres += bit_to_char(bit2);
                            if (true2 == 0)
                            {
                                tres += "'";
                            }
                            //tres += " + ";
                            //}
                            ttres.set_exp(tres);
                            if (ttres._flag != 0)
                            {
                                myret.Add(ttres);
                            }
                        }
                    }
                }
            }
            return myret;
        }
        // 两框
        private List<Sres> all_2bit()
        {
            List<Sres> myret = new List<Sres>();
            string tres = "";
            for (int bit1 = 3; bit1 >= 0; bit1--)
            {
                for (int bit2 = 0; bit2 < bit1; bit2++)
                {
                    for (int bit3 = 0; bit3 < bit2; bit3++)
                    {
                        for (int true1 = 0; true1 <= 1; true1++)
                        {
                            for (int true2 = 0; true2 <= 1; true2++)
                            {
                                for (int true3 = 0; true3 <= 1; true3++)
                                {
                                    tres = "";
                                    Sres ttres = new Sres();
                                    ttres = judge_2bit(bit1, true1, bit2, true2, bit3, true3);

                                    //if (judge_2bit(bit1, true1, bit2, true2, bit3, true3) != 0)
                                    //{
                                    tres += bit_to_char(bit1);
                                    if (true1 == 0)
                                    {
                                        tres += "'";
                                    }
                                    tres += bit_to_char(bit2);
                                    if (true2 == 0)
                                    {
                                        tres += "'";
                                    }
                                    tres += bit_to_char(bit3);
                                    if (true3 == 0)
                                    {
                                        tres += "'";
                                    }
                                    //    tres += " + ";
                                    //}
                                    ttres.set_exp(tres);
                                    if (ttres._flag != 0)
                                    {
                                        myret.Add(ttres);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return myret;
        }
        // 一框
        private string all_1bit()
        {
            string tres = "";
            for (int true1 = 0; true1 <= 1; true1++)
            {
                for (int true2 = 0; true2 <= 1; true2++)
                {
                    for (int true3 = 0; true3 <= 1; true3++)
                    {
                        for (int true4 = 0; true4 <= 1; true4++)
                        {
                            if (judge_1bit(true1, true2, true3, true4) != 0)
                            {
                                if (true1 == 0)
                                {
                                    tres += "Q3'";
                                }
                                else
                                {
                                    tres += "Q3";
                                }
                                if (true1 == 0)
                                {
                                    tres += "Q2'";
                                }
                                else
                                {
                                    tres += "Q2";
                                }
                                if (true1 == 0)
                                {
                                    tres += "Q1'";
                                }
                                else
                                {
                                    tres += "Q1";
                                }
                                if (true1 == 0)
                                {
                                    tres += "Q0'";
                                }
                                else
                                {
                                    tres += "Q0";
                                }
                                tres += " + ";
                            }
                        }
                    }
                }
            }
            return tres;
        }

        // 八框循环
        private Sres judge_8bit(int bit, int trues)
        {
            //int[] visit_back = new int[16];
            //for (int xi = 0; xi < 16; xi++) visit_back[xi] = visited[xi];
            List<int> _tlist = new List<int>();
            int flag = 1, bit1 = 0, bit2 = 0, bit3 = 0, position, i, j, k;
            delete_threes_bit(bit, ref bit1, ref bit2, ref bit3);
            int count_1 = 0;
            for (i = 0; i <= 1; i++)
            {
                for (j = 0; j <= 1; j++)
                {
                    for (k = 0; k <= 1; k++)
                    {
                        position = pow(2, bit) * trues + pow(2, bit1) * i + pow(2, bit2) * j + pow(2, bit3) * k;
                        if (a[position] == 0)
                        {
                            flag = 0;
                            break;
                        }
                        else if (a[position] == 1)
                        {
                            count_1++;
                            _tlist.Add(position);
                            visited[position] = 1;
                        }
                        else
                        {
                            visited[position] = 1;
                        }
                    }
                }
            }
            if (count_1 == 0)
            {
                flag = 0;
                //for (int xi = 0; xi < 16; xi++) visited[xi] = visit_back[xi];
            }
            Sres myr = new Sres(_tlist, flag);
            #region
            /*
            // 先判断它们在前面是否全被访问过
            if (flag == 1)
            {
                int flag2 = 1;
                for (i = 0; i <= 1; i++)
                {
                    for (j = 0; j <= 1; j++)
                    {
                        for (k = 0; k <= 1; k++)
                        {
                            position = pow(2, bit) * trues + pow(2, bit1) * i + pow(2, bit2) * j + pow(2, bit3) * k;
                            if (visited[position] == 0)
                            {
                                flag2 = 0;
                                break;
                            }
                        }
                    }
                }
                if (flag2 == 1)
                {
                    flag = 0;
                    //for (int xi = 0; xi < 16; xi++) visited[xi] = visit_back[xi];
                }
            }
            

            // 标记
            if (flag == 1)
            {
                for (i = 0; i <= 1; i++)
                {
                    for (j = 0; j <= 1; j++)
                    {
                        for (k = 0; k <= 1; k++)
                        {
                            position = pow(2, bit) * trues + pow(2, bit1) * i + pow(2, bit2) * j + pow(2, bit3) * k;
                            visited[position] = 1;
                        }
                    }
                }
            }
            
            
            */
            #endregion
            return myr;
        }
        // 四框循环
        private Sres judge_4bit(int bit1, int true1, int bit2, int true2)
        {
            //int[] visit_back = new int[16];
            //for (int xi = 0; xi < 16; xi++) visit_back[xi] = visited[xi];
            List<int> _tlist = new List<int>();
            int bit3 = 0, bit4 = 0; int flag = 1, position, i, j;
            int count_1 = 0;
            delete_two_bit(bit1, bit2, ref bit3, ref bit4);
            for (i = 0; i <= 1; i++)
            {
                for (j = 0; j <= 1; j++)
                {
                    position = pow(2, bit1) * true1 + pow(2, bit2) * true2 + pow(2, bit3) * i + pow(2, bit4) * j;
                    if (a[position] == 0)
                    {
                        flag = 0;
                        break;
                    }
                    else if (a[position] == 1)
                    {
                        count_1++;
                        _tlist.Add(position);
                        visited[position] = 1;
                    }
                    else
                    {
                        visited[position] = 1;
                    }
                }
            }
            if (count_1 == 0)
            {
                flag = 0;
                //for (int xi = 0; xi < 16; xi++) visited[xi] = visit_back[xi];
            }
            #region
            /*
            // 先判断它们在前面是否全被访问过
            if (flag == 1)
            {
                int flag2 = 1;
                for (i = 0; i <= 1; i++)
                {
                    for (j = 0; j <= 1; j++)
                    {
                        position = pow(2, bit1) * true1 + pow(2, bit2) * true2 + pow(2, bit3) * i + pow(2, bit4) * j;
                        if (visited[position] == 0)
                        { 
                            flag2 = 0;
                            break; 
                        }
                    }
                }
                if (flag2 == 1)
                {
                    flag = 0;
                    for (int xi = 0; xi < 16; xi++) visited[xi] = visit_back[xi];
                }
            }
            // 标记
            if (flag == 1)
            {
                for (i = 0; i <= 1; i++)
                {
                    for (j = 0; j <= 1; j++)
                    {
                        position = pow(2, bit1) * true1 + pow(2, bit2) * true2 + pow(2, bit3) * i + pow(2, bit4) * j;
                        visited[position] = 1;
                    }
                }
            }
            */
            #endregion

            Sres myr = new Sres(_tlist, flag);

            return myr;
        }
        // 两框循环
        private Sres judge_2bit(int bit1, int true1, int bit2, int true2, int bit3, int true3)
        {
            //int[] visit_back = new int[16];
            //for (int xi = 0; xi < 16; xi++) visit_back[xi] = visited[xi];
            List<int> _tlist = new List<int>();
            int bit4 = 0, flag = 1, position = 0, i;
            int count_1 = 0;
            delete_one_bit(bit1, bit2, bit3, ref bit4);
            for (i = 0; i <= 1; i++)
            {
                position = pow(2, bit1) * true1 + pow(2, bit2) * true2 + pow(2, bit3) * true3 + pow(2, bit4) * i;
                if (a[position] == 0)
                {
                    flag = 0;
                    break;
                }
                else if (a[position] == 1)
                {
                    count_1++;
                    _tlist.Add(position);
                    visited[position] = 1;
                }
                else
                {
                    visited[position] = 1;
                }
            }
            if (count_1 == 0)
            {
                flag = 0;
                //for (int xi = 0; xi < 16; xi++) visited[xi] = visit_back[xi];
            }

            #region
            /*
            // 先判断它们在前面是否全被访问过
            if (flag == 1)
            {
                int flag2 = 1;
                for (i = 0; i <= 1; i++)
                {
                    position = pow(2, bit1) * true1 + pow(2, bit2) * true2 + pow(2, bit3) * true3 + pow(2, bit4) * i;
                    if (visited[position] == 0)
                    {
                        flag2 = 0;
                    }
                }
                if (flag2 == 1)
                {
                    flag = 0;
                    for (int xi = 0; xi < 16; xi++) visited[xi] = visit_back[xi];
                }
            }
            // 标记
            if (flag == 1)
            {
                for (i = 0; i <= 1; i++)
                {
                    position = pow(2, bit1) * true1 + pow(2, bit2) * true2 + pow(2, bit3) * true3 + pow(2, bit4) * i;
                    visited[position] = 1;
                }
            }
             */
            #endregion

            Sres myr = new Sres(_tlist, flag);
            return myr;
        }
        // 一框循环
        private int judge_1bit(int true1, int true2, int true3, int true4)
        {
            int flag = 1, position;
            position = 8 * true1 + 4 * true2 + 2 * true3 + true4;
            if (a[position] == 0 || a[position] == 8)
            {
                flag = 0;
            }
            if (flag == 1)
            {
                if (visited[position] == 1)
                {
                    flag = 0;
                }
            }
            // 标记
            if (flag == 1)
            {
                visited[position] = 1;
            }
            return flag;
        }

        // 得到循环三个变量位置
        private void delete_threes_bit(int bit, ref int bit1, ref int bit2, ref int bit3)
        {
            if (bit == 3) { bit1 = 2; bit2 = 1; bit3 = 0; }
            else if (bit == 2) { bit1 = 3; bit2 = 1; bit3 = 0; }
            else if (bit == 1) { bit1 = 3; bit2 = 2; bit3 = 0; }
            else if (bit == 0) { bit1 = 3; bit2 = 2; bit3 = 1; }
        }
        // 得到循环两个变量位置
        private void delete_two_bit(int bit1, int bit2, ref int bit3, ref int bit4)
        {
            if (bit1 == 3 && bit2 == 2) { bit3 = 1; bit4 = 0; }
            else if (bit1 == 3 && bit2 == 1) { bit3 = 2; bit4 = 0; }
            else if (bit1 == 3 && bit2 == 0) { bit3 = 2; bit4 = 1; }
            else if (bit1 == 2 && bit2 == 1) { bit3 = 3; bit4 = 0; }
            else if (bit1 == 2 && bit2 == 0) { bit3 = 3; bit4 = 1; }
            else if (bit1 == 1 && bit2 == 0) { bit3 = 3; bit4 = 2; }
        }
        // 得到循环一个变量位置
        private void delete_one_bit(int bit1, int bit2, int bit3, ref int bit4)
        {
            if (bit1 == 3 && bit2 == 2 && bit3 == 1) { bit4 = 0; }
            else if (bit1 == 3 && bit2 == 2 && bit3 == 0) { bit4 = 1; }
            else if (bit1 == 3 && bit2 == 1 && bit3 == 0) { bit4 = 2; }
            else if (bit1 == 2 && bit2 == 1 && bit3 == 0) { bit4 = 3; }
        }

        // 幂运算
        private int pow(int _x, int _y)
        {
            return Convert.ToInt32(System.Math.Pow(_x, _y));
        }
        // 由数字代号转为相应的字符
        private string bit_to_char(int bit)
        {
            if (bit == 3) return "Q3";
            else if (bit == 2) return "Q2";
            else if (bit == 1) return "Q1";
            else return "Q0";
        }

        //====================== 以上是化简卡诺图用的私有函数 =========================//



        // 变量
        public uint[,] table = new uint[4, 4];
        private int[] a = new int[16];        //用来存放16个数
        private int[] visited = new int[16];  //化简过程中是否被用过
        private List<Sres> _sop = null;
    }


    // 交互器
    public class Service
    {
        // 私有的构造器
        private Service()
        {
            this.reuse();
        }
        // 单例函数
        public static Service get_Instance()
        {
            if (_flaggem == false)
            {
                _soulgem = new Service();
                _flaggem = true;
            }
            return _soulgem;
        }
        // 初始化函数
        public void reuse()
        {
            _homu = new List<Karno>();
            set_next_count = 4;
            _ntable = new uint[16, 8];
            this.初始化次态表();
        }

        //========================== 功能函数 ==========================//

        // 设置次态变量个数
        public uint set_next_count
        {
            get { return this._next_state; }
            set { this._next_state = value; }
        }
        // 初始化次态表
        public void 初始化次态表()
        {
            #region 十六状态的枚举
            _ntable[0, 0] = 0; _ntable[0, 1] = 0; _ntable[0, 2] = 0; _ntable[0, 3] = 0;
            _ntable[1, 0] = 0; _ntable[1, 1] = 0; _ntable[1, 2] = 0; _ntable[1, 3] = 1;
            _ntable[2, 0] = 0; _ntable[2, 1] = 0; _ntable[2, 2] = 1; _ntable[2, 3] = 0;
            _ntable[3, 0] = 0; _ntable[3, 1] = 0; _ntable[3, 2] = 1; _ntable[3, 3] = 1;
            _ntable[4, 0] = 0; _ntable[4, 1] = 1; _ntable[4, 2] = 0; _ntable[4, 3] = 0;
            _ntable[5, 0] = 0; _ntable[5, 1] = 1; _ntable[5, 2] = 0; _ntable[5, 3] = 1;
            _ntable[6, 0] = 0; _ntable[6, 1] = 1; _ntable[6, 2] = 1; _ntable[6, 3] = 0;
            _ntable[7, 0] = 0; _ntable[7, 1] = 1; _ntable[7, 2] = 1; _ntable[7, 3] = 1;
            _ntable[8, 0] = 1; _ntable[8, 1] = 0; _ntable[8, 2] = 0; _ntable[8, 3] = 0;
            _ntable[9, 0] = 1; _ntable[9, 1] = 0; _ntable[9, 2] = 0; _ntable[9, 3] = 1;
            _ntable[10, 0] = 1; _ntable[10, 1] = 0; _ntable[10, 2] = 1; _ntable[10, 3] = 0;
            _ntable[11, 0] = 1; _ntable[11, 1] = 0; _ntable[11, 2] = 1; _ntable[11, 3] = 1;
            _ntable[12, 0] = 1; _ntable[12, 1] = 1; _ntable[12, 2] = 0; _ntable[12, 3] = 0;
            _ntable[13, 0] = 1; _ntable[13, 1] = 1; _ntable[13, 2] = 0; _ntable[13, 3] = 1;
            _ntable[14, 0] = 1; _ntable[14, 1] = 1; _ntable[14, 2] = 1; _ntable[14, 3] = 0;
            _ntable[15, 0] = 1; _ntable[15, 1] = 1; _ntable[15, 2] = 1; _ntable[15, 3] = 1;
            #endregion
            // 其余位置为无关项
            for (int i = 4; i < 8; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    _ntable[j, i] = 8;
                }
            }
        }
        // 从队列录入次态表
        public void 录入次态表耶(Queue<uint> _org)
        {
            for (int i = 4; i < 8; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    _ntable[j, i] = _org.Dequeue();
                }
            }
        }
        // 读出次态表队列
        public Queue<uint> 读出次态表耶()
        {
            Queue<uint> _org = new Queue<uint>();
            for (int i = 4; i < 8; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    _org.Enqueue(_ntable[j, i]);
                }
            }
            return _org;
        }
        // 获得次态表输出队列
        public Queue<uint> 获得次态表耶()
        {
            Queue<uint> _qbuffer = new Queue<uint>();
            for (int i = 0; i < 16; i++)
            {
                for (int j = 4; j < 8; j++)
                {
                    _qbuffer.Enqueue(_ntable[i, j]);
                }
            }
            return _qbuffer;
        }
        // 计算逻辑相邻项循环项的函数
        private uint g(uint _org)
        {
            if (_org < 4) return _org;
            else return _org - 4;
        }
        // 计算下一逻辑相邻项的函数
        private uint f(uint _a, uint _b)
        {
            if (_a == 0 && _b == 0) return 0;
            else if (_a == 0 && _b == 1) return 1;
            else if (_a == 1 && _b == 1) return 2;
            else return 3;
        }
        // 计算J的函数
        private uint gj(uint _org, uint _dst)
        {
            if (_org == 0 && _dst == 0) return 0;
            else if (_org == 0 && _dst == 1) return 1;
            else return 8;
        }
        // 计算K的函数
        private uint gk(uint _org, uint _dst)
        {
            if (_org == 1 && _dst == 0) return 1;
            else if (_org == 1 && _dst == 1) return 0;
            else return 8;
        }
        // 打印卡诺图的函数
        public void pout(Karno _tmap, TextBlock _tbox)
        {
            _tbox.Text = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_tmap.table[i, j] == 8)
                    {
                        _tbox.Text += "X";
                    }
                    else
                    {
                        _tbox.Text += Convert.ToString(_tmap.table[i, j]);
                    }
                    if (j < 3)
                    {
                        _tbox.Text += "   ";
                    }
                }
                if (i < 3)
                {
                    _tbox.Text += Environment.NewLine;
                }
            }
        }
        // 生成卡诺图的函数
        public void homura()
        {
            _homu.Clear();
            for (uint nexts = 4; nexts < 4 + _next_state; nexts++)
            {
                Karno lab = new Karno();
                for (uint next_y = 0; next_y < 16; next_y++)
                {
                    lab.table[f(_ntable[next_y, 0], _ntable[next_y, 1]), f(_ntable[next_y, 2], _ntable[next_y, 3])] = gj(_ntable[next_y, nexts - 4], _ntable[next_y, nexts]);
                }
                _homu.Add(lab);
            }
            for (uint nexts = 4; nexts < 4 + _next_state; nexts++)
            {
                Karno lab = new Karno();
                for (uint next_y = 0; next_y < 16; next_y++)
                {
                    lab.table[f(_ntable[next_y, 0], _ntable[next_y, 1]), f(_ntable[next_y, 2], _ntable[next_y, 3])] = gk(_ntable[next_y, nexts - 4], _ntable[next_y, nexts]);
                }
                _homu.Add(lab);
            }
        }
        // 获得卡诺图输出序列
        public Queue<Karno> 获得输出卡诺序列()
        {
            Queue<Karno> _tout = new Queue<Karno>();
            foreach (Karno _item in _homu)
            {
                _tout.Enqueue(_item);
            }
            return _tout;
        }
        // 十进制转四位二进制
        public Fbit 十转二(uint _org)
        {
            uint _back_org = _org;
            Stack<uint> _mystack = new Stack<uint>();
            while (_org != 0)
            {
                _mystack.Push(Convert.ToUInt32(_org % 2));
                _org /= 2;
            }
            while (_mystack.Count < 4)
            {
                _mystack.Push(0);
            }
            return new Fbit(_mystack.Pop(), _mystack.Pop(), _mystack.Pop(), _mystack.Pop(), _back_org);
        }
        // 根据输入队列写次态表
        public void 单环写表(Queue<Fbit> _forg)
        {
            int _ctr = _forg.Count, _pct = 2;
            Fbit p1, p2, phead;
            p1 = _forg.Dequeue();
            p2 = _forg.Dequeue();
            phead = p1;
            do
            {
                for (int i = 4; i < 8; i++)
                {
                    _ntable[p1.fdec(), i] = p2.gbit(i - 4);
                }
                p1 = p2;
                if (_forg.Count != 0)
                {
                    p2 = _forg.Dequeue();
                }
                _pct++;
            } while (_pct < _ctr + 1);
            // 首尾相接
            for (int i = 4; i < 8; i++)
            {
                _ntable[p2.fdec(), i] = phead.gbit(i - 4);
            }
        }
        // 二环写表（D=0）
        public void 二环写表(Queue<Fbit> _forg, Fbit phead)
        {
            int _ctr = _forg.Count, _pct = 2;
            Fbit p1, p2;
            p1 = _forg.Dequeue();
            p1.set_state(1, p1.gbit(1), p1.gbit(2), p1.gbit(3), 8 + p1.fdec());
            p2 = _forg.Dequeue();
            p2.set_state(0, p2.gbit(1), p2.gbit(2), p2.gbit(3), p2.fdec());

            do
            {
                for (int i = 4; i < 8; i++)
                {
                    _ntable[p1.fdec(), i] = p2.gbit(i - 4);
                }
                p1 = p2;
                p1.set_state(1, p1.gbit(1), p1.gbit(2), p1.gbit(3), 8 + p1.fdec());
                if (_forg.Count != 0)
                {
                    p2 = _forg.Dequeue();
                    p2.set_state(0, p2.gbit(1), p2.gbit(2), p2.gbit(3), p2.fdec());
                }
                _pct++;
            } while (_pct < _ctr + 1);
            // 首尾相接
            for (int i = 4; i < 8; i++)
            {
                _ntable[p2.fdec(), i] = phead.gbit(i - 4);
            }
        }
        // 二环写表（D=1）
        public void 二环一(Queue<Fbit> _forg)
        {
            int _ctr = _forg.Count, _pct = 2;
            Fbit p1, p2, phead;
            p1 = _forg.Dequeue();
            p1.set_state(1, p1.gbit(1), p1.gbit(2), p1.gbit(3), 8 + p1.fdec());
            p2 = _forg.Dequeue();
            p2.set_state(1, p2.gbit(1), p2.gbit(2), p2.gbit(3), 8 + p2.fdec());
            phead = p1;
            do
            {
                for (int i = 4; i < 8; i++)
                {
                    _ntable[p1.fdec(), i] = p2.gbit(i - 4);
                }
                p1 = p2;
                p1.set_state(1, p1.gbit(1), p1.gbit(2), p1.gbit(3), p1.fdec());
                if (_forg.Count != 0)
                {
                    p2 = _forg.Dequeue();
                    p2.set_state(1, p2.gbit(1), p2.gbit(2), p2.gbit(3), 8 + p2.fdec());
                }
                _pct++;
            } while (_pct < _ctr + 1);
            // 首尾相接
            for (int i = 4; i < 8; i++)
            {
                _ntable[p2.fdec(), i] = phead.gbit(i - 4);
            }
        }
        // 二环写表（D=1 二轮）
        public void 二环一二环(Queue<Fbit> _forg, Fbit _phead)
        {
            Fbit phead = _phead;
            phead.set_state(1, phead.gbit(1), phead.gbit(2), phead.gbit(3), 8 + phead.fdec());
            int _ctr = _forg.Count, _pct = 2;
            Fbit p1, p2;
            p1 = _forg.Dequeue();
            p1.set_state(0, p1.gbit(1), p1.gbit(2), p1.gbit(3), p1.fdec());
            p2 = _forg.Dequeue();
            p2.set_state(1, p2.gbit(1), p2.gbit(2), p2.gbit(3), p2.fdec());

            do
            {
                for (int i = 4; i < 8; i++)
                {
                    _ntable[p1.fdec(), i] = p2.gbit(i - 4);
                }
                p1 = p2;
                p1.set_state(0, p1.gbit(1), p1.gbit(2), p1.gbit(3), p1.fdec());
                if (_forg.Count != 0)
                {
                    p2 = _forg.Dequeue();
                    p2.set_state(1, p2.gbit(1), p2.gbit(2), p2.gbit(3), p2.fdec());
                }
                _pct++;
            } while (_pct < _ctr + 1);
            // 首尾相接
            for (int i = 4; i < 8; i++)
            {
                _ntable[p2.fdec(), i] = phead.gbit(i - 4);
            }
        }
        // 双向反向写表
        public void 反向写表(Queue<Fbit> _forg)
        {
            int _ctr = _forg.Count, _pct = 2;
            Fbit p1, p2, phead;
            p1 = _forg.Dequeue();
            p1.set_state(1, p1.gbit(1), p1.gbit(2), p1.gbit(3), 8 + p1.fdec());
            p2 = _forg.Dequeue();
            p2.set_state(1, p2.gbit(1), p2.gbit(2), p2.gbit(3), 8 + p2.fdec());
            phead = p1;
            do
            {
                for (int i = 4; i < 8; i++)
                {
                    _ntable[p1.fdec(), i] = p2.gbit(i - 4);
                }
                p1 = p2;
                if (_forg.Count != 0)
                {
                    p2 = _forg.Dequeue();
                    p2.set_state(1, p2.gbit(1), p2.gbit(2), p2.gbit(3), 8 + p2.fdec());
                }
                _pct++;
            } while (_pct < _ctr + 1);
            // 首尾相接
            for (int i = 4; i < 8; i++)
            {
                _ntable[p2.fdec(), i] = phead.gbit(i - 4);
            }
        }
        // 把D填充无关项
        public void 填控制变量无关()
        {
            for (int i = 0; i < 16; i++)
            {
                this._ntable[i, 4] = 8;
            }
        }
        //========================== 以上是功能函数 ==========================//


        // 私有变量（当前案例）
        private uint _next_state = 4;

        // 内容变量
        private List<Karno> _homu = null;
        private uint[,] _ntable = null;

        // 静态变量
        private static Service _soulgem = null;
        private static Boolean _flaggem = false;
    }

}
