using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex4
{
    class A
    {
        public void m(int a) { Console.WriteLine("A.m"); }
        static void Main(string[] args)
        {

            A a = new A();

            WeakActionDelegate<int> wd = new WeakActionDelegate<int>(new Action<int>(a.m));
            wd.Invoke(1); // invoca m
            GC.Collect(); // delegate interno de wd é recolhido
            wd.Invoke(1); // delegate interno é de novo instanciado porque Target está vivo
            Console.WriteLine(a.ToString());
            GC.Collect(); // delegate interno de wd e objecto referido por A são recolhidos
            wd.Invoke(1); // não tem qualquer consequência
            Console.Read();
        }
    }

    internal class WeakActionDelegate<T>
    {
        WeakReference act;
        public WeakActionDelegate(Action<T> a)
        {
            act = new WeakReference(a);
        }
        public void Invoke(T arg)
        {
            if (act.IsAlive)
            {
                Action<T> action = (Action<T>)act.Target;
                action(arg);
            }
        }
    }
}

