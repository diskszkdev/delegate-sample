using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateApp
{
    delegate void SampleDelegate(int a);
    delegate void InstanceDelegate(string str);
    delegate void AnonymousDelegate(int a, int b);

    class Program
    {
        static void Main(string[] args)
        {
            // 基本
            SampleDelegate a = new SampleDelegate(AddOne);

            // 暗黙の型変換も可能
            SampleDelegate s = SubtractOne;

            // インスタンスメソッドも代入可能
            var sampleClass = new SampleClass();
            InstanceDelegate i = sampleClass.Show;

            // 複数のメソッドを代入可能(マルチキャストデリゲート)
            InstanceDelegate m = sampleClass.Show;
            m += sampleClass.Show;


            // 匿名関数を代入可能
            AnonymousDelegate an = delegate (int param1, int param2)
            {
                Console.WriteLine("Called Anonymouse Function!!!");
                Console.WriteLine($"result: {param1 + param2}");
                Console.WriteLine();
            };

            // ラムダ式でも書ける
            an += (int param1, int param2) =>
            {
                Console.WriteLine("Called Lambda Expression!!!");
                Console.WriteLine($"result: {param1 + param2}");
                Console.WriteLine();
            };

            // もっと簡単なラムダ式
            an += (param1, param2) =>
            {
                Console.WriteLine("Called Simple Lambda Expression!!!");
                Console.WriteLine($"result: {param1 + param2}");
                Console.WriteLine();
            };

            // ラムダ式の中身が一行ならさらに簡単に書ける
            an += (param1, param2) => Console.WriteLine("Called More Simple Lambda Expression!!!");


            // デリゲートを介してメソッドを呼び出す
            a(123);
            s(100);
            i("test");
            m("multi");
            an(5, 6);


            /* デリゲートにはすでに用意してある型がある(汎用デリゲート)
             * Actionは、任意個の引数を取り、かつ、戻り値のないメソッドへのDelegate
             * Funcは、任意個の引数を取り、かつ、戻り値のあるメソッドへのDelegate
             * Predicateは、任意個の引数を取り、かつ、戻り値の型がBooleanであるようなメソッドへのDelegate
             * ほかにもConvertorやComparisonがある
             */


            // 引数なし、戻り値あり
            Func<int> func_01 = () =>
             {
                 Console.WriteLine("Called Func<TResult> !!!");
                 Console.WriteLine();
                 return 0;
             };

            // 引数１つ、戻り値あり
            Func<int, string> func_02 = tt =>
            {
                var cal = tt + 2;
                Console.WriteLine("Called Func<T, TResult> !!!");
                Console.WriteLine($"result: {cal}");
                Console.WriteLine();
                return cal.ToString();
            };

            // 引数２つ、戻り値あり
            Func<int, int, string> func_03 = (aa, bb) =>
            {
                Console.WriteLine("Called Func<T1, T2, TResult> !!!");
                Console.WriteLine($"result: {aa + bb}");
                Console.WriteLine();
                return (aa + bb).ToString();
            };

            // 引数なし、戻り値なし
            Action action_01 = () => Console.WriteLine("Called Action !!!");

            // 引数１つ、戻り値なし
            Action<string> action_02 = str =>
            {
                Console.WriteLine("Called Action<T> !!!");
                Console.WriteLine($"param: {str}");
                Console.WriteLine();
            };

            // 引数２つ、戻り値なし
            Action<string, string> action_03 = (param1, param2) =>
            {
                Console.WriteLine("Called Action<T1, T2> !!!");
                Console.WriteLine($"param: {param1}, {param2}");
                Console.WriteLine();
            };

            // 引数１つ、戻り値Bool
            Predicate<int> predicate_01 = param =>
            {
                Console.WriteLine("Called Predicate<T> !!!");
                Console.WriteLine($"result: {param > 5}");
                return param > 5;
            };


            // 汎用メソッドを呼び出す
            func_01();
            func_02(8);
            func_03(56, 44);

            action_01();
            action_02("test");
            action_03("test01", "test02");

            predicate_01(9);


            // Linqをいろいろな呼び方をする
            var list = new List<string> { "aaa", "bbb", "ccc", "ddd", "eee", "fff" };

            var whereResult = list.Where(w => w == "ddd");
            whereResult = list.Where((w) => { return w == "ccc"; });
            whereResult = list.Where(sampleClass.LinqWhereFunction).ToList();

            var findResult = list.FindAll(x => x == "fff");
            findResult = list.FindAll(sampleClass.LinqFindFunction);

            Console.ReadKey();
        }

        private static void AddOne(int param)
        {
            Console.WriteLine("Called AddOne!!!");
            Console.WriteLine($"param: {param}");
            Console.WriteLine($"result: {param + 1}");
            Console.WriteLine();
        }

        private static void SubtractOne(int param)
        {
            Console.WriteLine("Called SubtractOne!!!");
            Console.WriteLine($"param: {param}");
            Console.WriteLine($"result: {param - 1}");
            Console.WriteLine();
        }
    }

    public class SampleClass
    {
        public void Show(string param)
        {
            Console.WriteLine("Called Show!!!");
            Console.WriteLine($"param: {param}");
            Console.WriteLine();
        }

        public bool LinqWhereFunction(string str)
        {
            Console.WriteLine("Called LinqWhereFunction!!!");
            return str == "eee";
        }

        public bool LinqFindFunction(string str)
        {
            Console.WriteLine("Called LinqFindFunction!!!");
            return str == "eee";
        }
    }
}