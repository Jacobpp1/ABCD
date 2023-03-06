using static System.Console;
using static System.Math;
public class vec{
	public double x,y,z;
	public vec(double a, double b, double c=10){x=a; y=b; z=c;}
	public void print(string s){Write(s);WriteLine($"{x},{y},{z}");}
	
	public static vec operator+(vec u, vec v){ return new vec(u.x+v.x, u.y+v.y, u.z+v.z);}
	
	public static vec operator*(vec u, double c){
		return new vec(c*u.x, c*u.y, c*u.z);
	}
	public static vec operator*(double c, vec u){
		return new vec(c*u.x, c*u.y, c*u.z);
	}
	
	public static vec operator-(vec u, vec v){return new vec(u.x-v.x, u.y-v.y, u.z-v.z);}
	public static vec operator-(vec u){return new vec(-u.x, -u.y, -u.z);}
	
	public static double operator%(vec u, vec v){ return u.x*v.x + u.y*v.y + u.z*v.z;} //dot product
	public double dot (vec other){return this%other;}
	
	public vec cross(vec v){return new vec(this.y*v.z-this.z*v.y, this.z*v.x-this.x*v.z, this.x*v.y-this.y*v.x);}
	public static vec operator&(vec u, vec v){return new vec(u.y*v.z-u.z*v.y, u.z*v.x-u.x*v.z, u.x*v.y-u.y*v.x);}
	
	public double norm(){return Sqrt(x*x + y*y + z*z);}

	static bool approx(double a,double b,double acc=1e-9,double eps=1e-9){
		if(Abs(a-b)<acc)return true;
		if(Abs(a-b)<(Abs(a)+Abs(b))*eps)return true;
		return false;
	}
	public bool approx(vec other){
		if(!approx(this.x,other.x)) return false;
		if(!approx(this.y,other.y)) return false;
		if(!approx(this.z,other.z)) return false;
		return true;
	}
	public static bool approx(vec u, vec v) => u.approx(v);

	public override string ToString(){ return $"{x}, {y}, {z}"; }
}
