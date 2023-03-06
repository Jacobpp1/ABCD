using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using static System.Math;
class main{
	public static int Main(string[] args){

		int nterms = (int)1e8, nthreads = 1;
		
		//2
		foreach(var arg in args){
			var words = arg.Split(':');
			if(words[0] == "-terms"){
				nterms = (int)float.Parse(words[1]);
			}
			if(words[0] == "-threads"){
				nthreads = (int)float.Parse(words[1]);
			}
		}
		// 3
		WriteLine($"nthreads={nthreads}, nterms={nterms}");
		data[] x = new data[nthreads];
		for(int i=0; i<nthreads; i++){
			x[i] = new data();
			x[i].a = 1 + i*nterms/nthreads;
			x[i].b = 1 + (i+1)*nterms/nthreads;
			WriteLine($"x.a={x[i].a}, x.b={x[i].b}, i={i}");
		}

		//4
		Thread[] threads = new Thread[nthreads];
		for(int i=0; i<nthreads; i++){
			threads[i] = new Thread(harm);
			threads[i].Name = $"thread number {i+1}";
			threads[i].Start(x[i]);
		}
		
		//5
		for(int i=0; i<nthreads; i++){
			threads[i].Join();
		}

		//6
		double total = 0;
		for(int i=0; i<nthreads; i++){
			total += x[i].sumab;
		}
		WriteLine($"Total sum = {total}");
		double sum=0;
	    Parallel.For( 1, nterms+1, delegate(int i){sum+=1.0/i;} );
		WriteLine($"Parallel.For sum = {sum}");
		WriteLine("___________");
		
		//8
		WriteLine("Time for one thread: Real 1.30, user 7.11, sys 0.00");
		WriteLine("Time for one thread without Parallel.For: Real 0.45, user 0.44, sys 0.00\n");
		
		WriteLine("Time for two threads: Real 1.07, user 7.14, sys 0.00");
		WriteLine("Time for two threads without Parallel.For: Real 0.23, user 0.45, sys 0.00\n");
		
		WriteLine("Time for three threads: Real 1.04, user 7.44, sys 0.00");
		WriteLine("Time for three threads without Parallel.For: Real 0.17, user 0.49, sys 0.00\n");

		WriteLine("Time for four threads: Real 0.98, user 7.22, sys 0.00");
		WriteLine("Time for four threads without Parallel.For: Real 0.13, user 0.49, sys 0.00\n");
		
		WriteLine("Parallel.For is probably slower (and wrong) because it tries to automatically split up and calculate the different terms and then adding them");
		WriteLine("But when they are added, the sum variable can already have been changed from another thread.");
		WriteLine("Thus the time taken is increased because all the threads must find the same global variable every time to add.")
		return 0;
	}


	//1
	public class data{
		public int a,b;
		public double sumab;
	}

	public static void harm(object obj){
		data x = (data)obj;		
		//WriteLine($"{Thread.CurrentThread.Name}, a={x.a}, b={x.b}");
		x.sumab = 0;
		for(int i=x.a; i<x.b; i++){
			x.sumab += 1.0/i;
		}
		//WriteLine($"{Thread.CurrentThread.Name} partial sum={x.sumab}");
	}

}
