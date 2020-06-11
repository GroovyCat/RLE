using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLE
{
    class Rle_test
    {
        public static void Main(string[] args)
        {
            byte[] input = { 0x5, 0x5, 0x6, 0x55, 0x8, 0x7, 0x6, 0x7, 0x5, 0x5, 0x5, 0x6, 0x6, 0x55, 0x55}; // 입력 데이터
            List<byte> enco_data = new List<byte>(); // 인코딩 데이터 
            List<byte> deco_data = new List<byte>(); // 디코딩 데이터

            RLE r = new RLE(); // RLE class 객체 생성
            r.Encode(input, enco_data); // 인코딩
            r.Decode(enco_data, deco_data); //디코딩

            Console.Write("Input : ");
            r.ResultBasic(input); // 입력 데이터 결과 확인
            Console.WriteLine();
            Console.Write("Encoding : ");
            r.ResultEnco(enco_data); // 인코딩 결과 확인
            Console.WriteLine();
            Console.Write("Decoding : ");
            r.ResultDeco(deco_data); // 디코딩 결과 확인
            Console.WriteLine();
        }
    }
    class RLE
    {
        // 인코딩 메서드
        public void Encode(byte[] input, List<byte> enco_data)
        {
            byte escape = 0x55; // escape : 0x55
            int i = 0; 
            int count = 0;
            while(i < input.Length)
            {
                if(input[i] == escape) // 0x55일때 escape 2번 인코딩
                {
                    enco_data.Add(escape);
                    enco_data.Add(escape);
                }
                else
                {
                    count = 1;
                    while((i + count) < input.Length && input[i] == input[i + count])
                    {
                        count++;
                    }
                    if(count >= 3) // 0x5가 연속 3개 이상일때 0x55 해당 배열 공간의 데이터, 0x5의 개수를 표현한 0xcount
                    {
                        enco_data.Add(escape);
                        enco_data.Add(input[i]);
                        enco_data.Add((byte)count);
                        i += (count - 1);
                    }
                    else
                    {
                        enco_data.Add(input[i]); // 0x5가 연속 2개 or 하나 or 나머지 값
                    }
                }
                i++;
            }
        }
        // 디코딩 메서드
        public void Decode(List<byte> enco_data, List<byte> deco_data)
        {
            byte count = 0;
            byte i = 0;
            byte k = 0;
            byte co_num;
            byte num;
            byte escape = 0x55; // escape : 0x55
            byte[] arr = new byte[enco_data.Count];

            foreach(byte j in enco_data)
            {
                arr[count++] = j;
            }
            while(i < arr.Length)
            {
                if(arr[i] == escape) // 0x55일때 
                {
                    if(arr[i + 1] == escape) // 다음 배열 칸의 데이터가 0x55일때
                    {
                        deco_data.Add(arr[i]); // 0x55를 디코딩 리스트에 저장
                        i++;
                    }
                    else 
                    {
                        num = arr[i + 1]; // 0x55 3개 이상의 인코딩 결과의 가운데 데이터 num에 저장
                        co_num = (byte)arr[i + 2]; // 0x5의 개수를 표현한 0xcount를 co_num에 저장
                        while(k < co_num) 
                        {
                            deco_data.Add(num); // num을 디코딩 리스트에 저장
                            k++;
                        }
                        i += 2;
                    }
                }
                else // 0x5가 연속 2개 or 1개 or 나머지 값
                {
                    deco_data.Add(arr[i]);
                }
                i++;
            }
        }
        // 결과확인 메서드
        public void ResultBasic(byte[] input)
        {
            foreach (byte i in input)
            {
                Console.Write("0x{0:X2} ", i);
            }
        } 
        public void ResultEnco(List<byte> enco_data)
        {
            foreach (byte i in enco_data)
            {
                Console.Write("0x{0:X2} ", i);
            }
        }
        public void ResultDeco(List<byte> deco_data)
        {
            foreach (byte i in deco_data)
            {
                Console.Write("0x{0:X2} ", i);
            }
        }
    }
}
        