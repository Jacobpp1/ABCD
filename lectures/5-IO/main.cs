using System;
using static System.Console;
using static System.Math;
class main{
	public static int Main(string[] args){
		WriteLine("Hello");
		// Lecture stuff
		/*string infile = null; string outfile = null;
		foreach(string arg in args){
			WriteLine(arg);
			var words = arg.Split(':');
			if (words[0] == "-input") infile = words[1];
			if (words[0] == "-output") outfile = words[1];
		}
		if (infile == null) {Error.WriteLine("no input file"); return 1;}
		double[] numbers = input.get_numbers_from_args(args);
		foreach(double number in numbers){
			WriteLine($"{number:0.00e+00}");
		}
		Error.WriteLine("return code 0");
		var inputstream = new System.IO.StreamReader(infile);
		var outputstream = new System.IO.StreamWriter(outfile,append:false);
		for(string line = inputstream.ReadLine(); line != null; line=inputstream.ReadLine()){
			double x = double.Parse(line);
			outputstream.WriteLine($"{x} {Sin(x)} {Cos(x)}");
			
		}
		inputstream.Close();
		outputstream.Close();

		for (string line = In.ReadLine(); line!= null; line = In.ReadLine()){
			double x = 2;
		}
		return 0;*/
		
		// 1
		foreach(var arg in args){
			var words = arg.Split(':');
			if(words[0]=="-numbers"){
				var numbers=words[1].Split(',');
				foreach(var number in numbers){
					double x = double.Parse(number);
					WriteLine($"{x} {Sin(x)} {Cos(x)}");
				}
			}
		}
		// 2
		//WriteLine("Test2");
		char[] split_delimiters = {' ','\t','\n'};
		var split_options = StringSplitOptions.RemoveEmptyEntries;
		for( string line = ReadLine(); line != null; line = ReadLine() ){
			var numbers = line.Split(split_delimiters,split_options);
			foreach(var number in numbers){
				double x = double.Parse(number);
				WriteLine($"{x} {Sin(x)} {Cos(x)}");
			}
        	}
		// 3
		//WriteLine("Test3");
		string infile=null,outfile=null;
		foreach(var arg in args){
			var words=arg.Split(':');
			if(words[0]=="-input")infile=words[1];
			if(words[0]=="-output")outfile=words[1];
		}
		if( infile==null || outfile==null) {
			Error.WriteLine("wrong filename argument");
			return 1;
		}
		var instream =new System.IO.StreamReader(infile);
		var outstream=new System.IO.StreamWriter(outfile,append:false);
		for(string line=instream.ReadLine();line!=null;line=instream.ReadLine()){
			double x=double.Parse(line);
			WriteLine($"Hello, {x}.");
			outstream.WriteLine($"{x} {Sin(x)} {Cos(x)}");
		}
		instream.Close();
		outstream.Close();

		//Complex
		complex neg_one = new complex(-1,0);
		WriteLine($"Complex\nsqrt(-1)={cmath.sqrt(neg_one)}");
		WriteLine($"sqrt(i)={cmath.sqrt(cmath.I)}");
		WriteLine($"exp(i)={cmath.exp(cmath.I)}");
		WriteLine($"exp(i*pi)={cmath.exp(cmath.I*PI)}");
		WriteLine($"i^i={cmath.pow(cmath.I,cmath.I)}");
		WriteLine($"ln(i)={cmath.log(cmath.I)}");
		WriteLine($"sin(i*pi)={cmath.sin(cmath.I*PI)}");
		return 0;
	}
}
