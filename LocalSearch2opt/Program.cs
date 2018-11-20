using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalSearch2opt
{
    class Dinh
    {
        private int so;
        private int x;
        private int y;

        public int So
        {
            get
            {
                return so;
            }
            set
            {
                so = value;
            }
        }
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public Dinh()
        {
            so = 0;
            x = 0;
            y = 0;
        }

        public int tinhDoDai(Dinh dinh2)
        {
            int dx, dy, d;
            dx = x - dinh2.x;
            dy = y - dinh2.y;
            d = Convert.ToInt32(Math.Sqrt(dx * dx + dy * dy));
            return d;
        }
    }

    class Program
    {
        static public List<Dinh> docFile(string tenFile)
        {
            List<Dinh> duongDi = new List<Dinh>();
            StreamReader sr = new StreamReader(tenFile);           
            string line;                
            while ((line = sr.ReadLine()) != null)
            {
                if (line == "EOF") break;
                string[] dong = line.Split();
                if (dong[0].ToString() == "NAME" || dong[0].ToString() == "COMMENT" || dong[0].ToString() == "TYPE" || dong[0].ToString() == "DIMENSION" || dong[0].ToString() == "EDGE_WEIGHT_TYPE" || dong[0].ToString() == "NODE_COORD_SECTION") continue;
                Dinh dinh = new Dinh();
                dinh = ganDinh(line);
                if(dinh.So == 0)
                {
                    Console.Write("Dữ liệu không hợp lệ !");
                }
                duongDi.Add(dinh);
            }

            return duongDi;
        }

        static Dinh ganDinh(string line)
        {
            string[] data;
            data = line.Split();
            Dinh dinh = new Dinh();
            dinh.So = Convert.ToInt32(data[0]);
            dinh.X = Convert.ToInt32(data[1]);
            dinh.Y = Convert.ToInt32(data[2]);
            return dinh;
        }

        static public int tinhDoDai(List<Dinh> duongDi)
        {
            int doDai = 0;
            Dinh dinh = new Dinh();
            foreach (Dinh node in duongDi)
            {
                doDai = doDai + node.tinhDoDai(dinh);
                dinh = node;
            }
            return doDai;
        }

        static public List<Dinh> phepDao_2opt(List<Dinh> duongDi, int i, int k)
        {
            List<Dinh> duongDiMoi = new List<Dinh>();
            for(int a = 0; a < i; a++)
            {
                duongDiMoi.Add(duongDi[a]);
            }

            for(int a = k; a >= i ; a--)
            {
                duongDiMoi.Add(duongDi[a]);
            }

            for(int a = k + 1; a < duongDi.Count; a++)
            {
                duongDiMoi.Add(duongDi[a]);
            }
            return duongDiMoi;
        }

        static public List<Dinh> Chay_2opt(List<Dinh> duongDi)
        {
            List<Dinh> duongDiTotNhat = new List<Dinh>();
            duongDiTotNhat = duongDi;
            int doDaiNganNhat = tinhDoDai(duongDi);
            int caiThien = 0;
            while (caiThien < 20)
            {
                //caiThien = false;
                for(int i = 0; i < duongDiTotNhat.Count - 1; i++)
                {
                    for (int k = i+1; k < duongDiTotNhat.Count; k++)
                    {
                        List<Dinh> duongDiMoi = new List<Dinh>();
                        duongDiMoi = phepDao_2opt(duongDiTotNhat,i,k);
                        int doDaiMoi = tinhDoDai(duongDiMoi);
                        if (doDaiMoi < doDaiNganNhat)
                        {
                            caiThien = 0;
                            doDaiNganNhat = doDaiMoi;
                            duongDiTotNhat = duongDiMoi;
                            //caiThien = true;                         
                        }
                    }
                }
                caiThien++;
            }

            return duongDiTotNhat;
        }

        static void Main(string[] args)
        {
            List<Dinh> duongDi = new List<Dinh>();
            List<Dinh> duongDiNgauNhien = new List<Dinh>();
            string tenFile = "";
            Stopwatch st = new Stopwatch();
            //Console.Write("Nhap ten file test: ");
            //tenFile = Console.ReadLine();
            tenFile = "filetest.txt";

            duongDi = docFile(tenFile);

            Random ngauNhien = new Random();
            int dinhBatDau = ngauNhien.Next(1, duongDi.Count - 1);

            for (int i = dinhBatDau; i < duongDi.Count; i++)
            {
                duongDiNgauNhien.Add(duongDi[i]);
            }
            for (int i = 0; i < dinhBatDau; i++)
            {
                duongDiNgauNhien.Add(duongDi[i]);
            }            

            duongDi = duongDiNgauNhien;

            st.Start();
            duongDi = Chay_2opt(duongDi);
            st.Stop();
            /*
            for(int i = 0; i < duongDi.Count; i++)
            {
                Console.WriteLine(duongDi[i].So);
            }
            */
            Console.WriteLine("So đinh: " + duongDi.Count);
            Console.WriteLine("Do dai: " + tinhDoDai(duongDi));
            Console.WriteLine("Thoi gian chay: {0} giay", (st.ElapsedMilliseconds / 1000).ToString());
            Console.ReadLine();
        }
    }
}
