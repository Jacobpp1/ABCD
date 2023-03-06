using static System.Console;
class main{
	static void Main(string[] args){
		string func = null;

		foreach(var arg in args){
			var words = arg.Split(':');
			if(words[0] == "-func"){
				func = words[1];
			}
		}
		if(func == "gamma"){
			for(double x=-5+1.0/128;x<=5;x+=1.0/64){
				WriteLine($"{x} {sfuns.gamma(x)}");
			}
		}

		if(func == "lngamma"){
			for(double x=0 + 1.0/128; x<=15; x+=1.0/64){
				WriteLine($"{x} {sfuns.lngamma(x)}");
			}
		}
		
		if(func == "error"){
			for(double x=-3.5+1.0/128; x<=3.5; x+=1.0/64){
				WriteLine($"{x} {sfuns.erf(x)}");
			}
		}

	}
}