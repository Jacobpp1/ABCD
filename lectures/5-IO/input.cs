static public class input{
	public static double[] get_numbers_from_args(string[] args){
		double[] result = null;
		foreach(string arg in args){
			string[] words = arg.Split(':');
			if(words[0]=="-numbers"){
				string[] numbers = words[1].Split(',');
				result = new double[numbers.Length];
				for(int i=0; i<numbers.Length; i++){
					result[i] = double.Parse(numbers[i]);
				}
			}
		}
		return result;
	}
}
