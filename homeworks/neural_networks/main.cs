using System;
using static System.Math;
using System.IO;
using static System.Console;

class main{
    public static void Main(){
        WriteLine("Problem A:");
        Func<double, double> g = (x) => Cos(5*x-1)*Exp(-x*x);
        int datapoints = 50;
        vector xs = new vector(datapoints);
        vector ys = new vector(datapoints);
        //for(int i = 0; i<xs.size; i++) ys[i] = g(xs[i]);
        for(int i=0; i<xs.size; i++){xs[i] = -1 + 2.0*(i+1)/datapoints; ys[i] = g(xs[i]);};
        string toWrite = $"";
        for(int i=0; i<xs.size; i++) toWrite += $"{xs[i]}\t{ys[i]}\n";
        File.WriteAllText("func.data", toWrite);
        
        toWrite = $"";
        ann network = new ann(4);
        network.train(xs,ys);
        for(int i=0; i<xs.size; i++) toWrite += $"{xs[i]}\t{network.response(xs[i], network.p)}\n";
        File.WriteAllText("A_fit.data", toWrite);

        WriteLine("Problem B:");
        WriteLine("Same problem but now including derivatives and anti-derivative");        
        toWrite = $"";
        for(int i=0; i<xs.size; i++) toWrite += $"{xs[i]}\t{network.response(xs[i], network.p)}\t{network.first_der(xs[i])}\t{network.second_der(xs[i])}\t{network.anti_der(xs[i])}\n";
        File.WriteAllText("B_fit.data", toWrite);
        
        WriteLine("Problem C:");
        
    }

    ///////////////////////// Minimization
    //public static vector fit(Func<vector, double> f, vector guess, List<double> xs, List<double> ys, List<double> errs, double acc){
    public static vector fit(Func<vector, double> f, vector guess, vector xs, vector ys, vector errs, double acc){
        Func<vector, double> deviation = (x) => {
            double sum = 0; for(int i=0; i<xs.size; i++) sum += Pow((f(new vector(xs[i], x[0], x[1], x[2])) - ys[i])/errs[i], 2); return sum;
        };
        return qnewton(deviation, guess, acc).Item1;
    }

    public static (vector, int) qnewton(
	Func<vector,double> f, /* objective function */
	vector start, /* starting point */
	double acc /* accuracy goal, on exit |gradient| should be < acc */
    ){
        int counter = 0;
        vector dx;
        vector s;
        vector u;
        vector y;
        vector grad_old;
        double lambda;
        double uy;
        matrix dB;
        int n = start.size;
        matrix B = new matrix(n,n);
        B.set_identity();
        vector grad = num_grad(f, start);
        // Maybe check that start is nonzero
        for(int i=0; i<n; i++){
            if(Abs(start[i]) < Pow(2,-26)) start[i] = Pow(2,-26);
        }
        while(grad.norm() > acc){
            counter++;
            lambda = 1.0;
            dx = -B*grad;
            while(true){
                s = lambda*dx;
                if (f(start+s) < f(start)){
                    start = start + s;
                    grad_old = grad;
                    grad = num_grad(f,start);
                    y = grad - grad_old;
                    u = s - B*y;
                    uy = u.dot(y); 
                    if(Abs(uy) > 1e-6){
                        // avoiding dividng by zero
                        dB = matrix.outer(u,u)/uy;
                        B = B + dB;
                    }
                    break;
                }
                lambda /= 2;
                if(lambda < Pow(2,-16)){
                    start = start + lambda*dx;
                    grad = num_grad(f,start);
                    B.set_identity();
                    break;
                }
            }
        }
        return (start, counter);
    }

    public static vector num_grad(Func<vector, double> f, vector x){
        int dim = x.size;
        vector grad = new vector(dim);
        for(int i = 0; i < dim; i++){
            vector newx = x.copy();
            double dx = Abs(x[i])*Pow(2,-26);
            newx[i] = x[i] + dx;
            grad[i] = (f(newx) - f(x))/dx;
        }
        return grad;
    }
    /////////////////////////////// End minimization
}

public class ann{
    int n; /* number of hidden neurons */
    Func<double,double> f = x => x*Exp(-x*x); /* activation function */
    public vector p; /* network parameters */
    //ann(int n){/* constructor */}
    public ann(int i) => n = i;
    public double response(double x, vector ps = null){
        /* return the response of the network to the input signal x */
        double F_p = 0;
        for(int i=0; i<n; i++) F_p += f((x-ps[i*3+0])/ps[i*3+1]) * ps[i*3+2];
        return F_p;
    }
    public void train(vector x,vector y){
        /* train the network to interpolate the given table {x,y} */
        Func<vector, double> cost2 = (p_temp) => {
            double sum = 0;
            for(int i=0; i<x.size; i++) sum += Pow(response(x[i],p_temp)-y[i], 2);
            return sum;
        };
        vector guess = new vector((int)(3*n));
        for(int i = 0; i<guess.size; i++){
            if(i%3 == 0) guess[i] = i*(x[x.size-1]-x[0])/n + x[0];
            else guess[i] = 1.0;
        }
        p = main.qnewton(cost2, guess, 1e-4).Item1;
    }

    public double first_der(double xs){
        Func<double,double> der_one = x => Exp(-x*x)*(1-2*x*x);
        double der = 0;
        for(int j = 0; j<n; j++) der += der_one((xs-p[j*3+0])/p[j*3+1]) * p[j*3+2];
        return der;
    }
    public double second_der(double xs){
        Func<double,double> der_two = x => 2*Exp(-x*x)*x*(2*x*x-3);
        double der = 0;
        for(int j = 0; j<n; j++) der += der_two((xs-p[j*3+0])/p[j*3+1]) * p[j*3+2];
        return der;
    }
    public double anti_der(double xs){
        Func<double,double> der_anti = x => -0.5*Exp(-x*x);
        double der = 0;
        for(int j = 0; j<n; j++) der += der_anti((xs-p[j*3+0])/p[j*3+1]) * p[j*3+2];
        return der;
    }
}