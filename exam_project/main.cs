using System;
using System.IO;
using static System.Console;
using static System.Math;
using static System.Random;

/*
CURRENT PROBLEMS:
- Error estimation seems bad when integrating 1/sqrt(x) - error never becomes smaller than tolerance.
- Average error method uses suspiciously few function calls. Seems to oscillating.
- Predefined nodes method seems to perform better. Better accuracy and fewer function calls.
*/

class main{
    static int N_ite;
    public static void Main(){
        N_ite = 0;
        WriteLine("------------------- Testing implementation of integration method -------------------");
        Func<double, double> f1 = x => x*x+2*x;
        WriteLine("Testing integration method on f(x)=2x+x*x from 0 to 5. Delta and epsilon set to 1e-3. N=10");
        double test = integrate(f1, 0, 5);
        WriteLine($"Theoretically we expect to get x*x + 1/3*x*x*x, evaluated gives: 25+1/3*125={25+1.0/3*125}. Integration method gives: {test}");
        test = integrate(f1, 0, 5, 1e-5, 1e-5, 40);
        WriteLine($"Integrating with delta and epsilon 1e-5 and N=40 instead gives {test}, which is an improvement\n");

        WriteLine($"Now integrating 1/sqrt(x) from 0 to 1. But this diverges so I use Clenshaw-Curtis transformation instead");
        f1 = (x) => 1.0/Sqrt(x);
        double test2 = integrate_CCtransform(f1, 0, 1);
        WriteLine($"We expect this to give 2. With delta and eps = 1e-3 and N=10 again we get: {test2}");
        f1 = x => Sin(x)*Exp(-x*x+x)*x;
        double test3 = integrate(f1, 0, PI, 1e-7, 1e-7, 40);
        WriteLine($"Testing integration on sin(x)exp(-x*x+x)*x from 0 to pi, delta and eps = 1e-7 and N=40 gives {test3}");
        WriteLine("Should give 1.068621813 according to WolframAlpha. So this is pretty good.\n");

        WriteLine("------------- Investigating dependence on number of new points thrown / optimum N -----------------");
        // Number of function calls / Optimal N
        f1 = x => Cos(x-1)*Sin(x);
        (int lowest, int best_N) = optimum_N(f1, -PI, PI, 2, 100);
        optimum_N(f1, -PI, PI, 2, 100, ave:true);
        optimum_N(f1, -PI, PI, 2, 100, CC:true);
        optimum_N(f1, -PI, PI, 2, 100, ave:true, CC:true);
        //WriteLine($"Lowest N_f/N: {lowest};\tBest N: {best_N}");
        //WriteLine($"Integral using best N: {integrate(f1, -PI, PI, N:best_N)}");
        WriteLine("Figure showing number of function calls versus number of new points in an iteration are plotted. Both as absolute and relative number of calls.");
        WriteLine("Absolute number of calls can be seen in 'func_calls.svg', while relative (N_calls/N) can be seen in 'opt_N.svg'");
        WriteLine("Number of iterations versus number of new points is shown in 'iterations.svg' along with number of function calls;");
        WriteLine("these are of course correlated, since each iteration leads to two function calls per point because of the subdivision.");
        WriteLine("In all cases, the integral is cos(x-1)*sin(x); integrated from -PI to PI with delta and epsilon both being 1e-3\n");
        WriteLine("From the figure showing number of iterations, we see that somewhere between N=30 and N=40 we get diminishing returns, this may be considered an optimum N.");
        WriteLine("Similar is also seen in figure 'opt_N.svg', showing function calls divided by N.");
        WriteLine("The difference is not significant, though, and thus N=10 has been chosen for other calculations unless specified.");
        WriteLine("");
        WriteLine("Surprisingly, the method of error calculation using the difference in mean leads to large variation in the integral value,");
        WriteLine("and therefore this is not considered a viable method\n");


        // Predefined node comparison:
        // Accuracy
        WriteLine("------------- Comparison with method from integration homework, using predefined nodes -----------------");
        f1 = x => 1.0/Sqrt(x);
        double test4 = hw_CC_integrate(f1, 0, 1);
        WriteLine($"Integrating 1/sqrt(x) from 0 to 1 with CC transformation. Cannot solve diverging integral without the transformation.");
        WriteLine("Should give 2. Integration methods give Random nodes: {test2}; Predefined nodes: {test4}");
        WriteLine($"Doing it 100 times and taking average difference, Abs(integrate(f) - 2)");
        double diff_rand = 0;
        double diff_pred = Abs(test4-2);
        for(int i = 0; i<100; i++) diff_rand += Abs(integrate_CCtransform(f1,0,1)-2);
        WriteLine($"Random nodes: {diff_rand/100}; Predefined nodes: {diff_pred}");
        WriteLine("So precision isn't increased with random nodes method. Maybe number of function calls is.\n");
        
        // Number of function calls
        int N_calls = 0;
        Func<double, double> f1_count = x => {N_calls++; return f1(x);};
        for(int i = 0; i<100; i++) integrate_CCtransform(f1_count,0,1);
        double N_calls_rand = N_calls / (double)100;
        N_calls = 0;
        hw_CC_integrate(f1_count,0,1);
        int N_calls_pred = N_calls;
        WriteLine($"Average number of function calls: Random nodes: {N_calls_rand}; Predefined nodes: {N_calls_pred}");
        WriteLine("So we also see that the efficiency in number of function calls is much better for the predefined nodes method.");

        // Test on other integral
        //f1 = x => Tan(x)*Cos(x)*Exp(-x);
        //f1 = x => {if(x<0.00001) return 0; else return 1/Sqrt(x);};
        f1 = x => 1/Sqrt(x);
        N_calls = 0;
        double res_rand = integrate_CCtransform(f1_count, 1e-6, 1);
        N_calls_rand = N_calls;
        N_calls = 0;
        int N_calls_noCC = 0;
        double res_rand_noCC = integrate(f1_count, 1e-6, 1);
        N_calls_noCC = N_calls;
        N_calls = 0;
        double res_pred = hw_CC_integrate(f1_count, 1e-6, 1);
        N_calls_pred = N_calls;
        WriteLine("\nNow with f(x)=1/sqrt(x), integrating from 1e-6 to 1.");
        WriteLine("Integration limit changed to allow the method without CC transform to work");
        WriteLine("so we can see if the CC transformation decreases function calls for divergent functions.");
        WriteLine($"Number of function calls: Random nodes: {N_calls_rand}; Random nodes (without CC transform): {N_calls_noCC}; Predefined nodes: {N_calls_pred}");
        WriteLine("Still better to use predefined nodes, but the CC transform has a significant effect.");
        WriteLine("So for divergent functions, the CC transform has fewer function calls, while it makes more function calls for simpler functions (see 'func_calls.svg')");
        //WriteLine($"Random nodes gives: {res_rand}; Predefined nodes gives: {res_pred}. Actual = 1.998");
    }

    static double integrate (Func<double,double> f, double a, double b, double delta=1e-3, double eps=1e-3,
                             double old_sum=0, double old_sum2=0, int old_n = 0, int N = 10, bool ave = false){
        Random rand = new Random(); // Seed randomiser for testing
        int n_left = 0, n_right = 0;
        double h = b-a;
        double x, fx, fx2;
        double sum = 0, sum2 = 0, sumleft = 0, sumright = 0, sumleft2 = 0, sumright2 = 0;
        int bad_N = 0;
        for(int i = 0; i<N; i++){
            x = a + h*rand.NextDouble();
            fx = f(x); fx2 = fx*fx;
            if(fx == 1.0/0){
                //WriteLine($"{fx} , {x}");
                bad_N++;
                continue;
            }
            //WriteLine(fx);
            sum += fx; sum2 += fx2;
            if(x < a + h/2.0){
                n_left++;
                sumleft += fx;
                sumleft2 += fx2;
            }
            else{
                n_right++;
                sumright += fx;
                sumright2 += fx2;
            }
        }
        //WriteLine(bad_N);
        sum += old_sum; sum2 += old_sum2;

        int N_total = N + old_n - bad_N;

        double mean = sum/N_total; double mean2 = sum2/N_total;
        double old_mean = old_sum/old_n;
        double integral = h*mean;                       // Equation 2, MC note
        double sigma = Sqrt(mean2 - mean*mean);         // Equation 4, MC note
        double error = h*sigma/Sqrt(N_total);           // Equation 3, MC note
        if(ave) error = h*Abs(mean-old_mean);           // From exam problem
        double tol = delta + eps*Abs(integral);         // Equation 47, integration note
        //WriteLine($"Error: {error}; tolerance: {tol}");
        
        if(error < tol) return integral;                // Equation 47, integration note
        else{
            N_ite++;
            return integrate(f, a, a+h/2, delta/Sqrt(2), eps, sumleft, sumleft2, n_left, N, ave) 
                 + integrate(f, a+h/2, b, delta/Sqrt(2), eps, sumright, sumright2, n_right, N, ave); // Accuracy goals: bottom page 9, integration note
        }
    }

    public static double integrate_CCtransform(Func<double, double> f, double a, double b, double delta = 0.001, double eps = 0.001, int N = 10, bool ave=false){
        return integrate(x => f((a+b)/2+(b-a)/2*Cos(x))*Sin(x)*(b-a)/2, 0, PI, delta, eps, N:N, ave:ave);
    }

    // Find best N by looking at number of function evaluations. Want lowest function calls per N.
    public static (int, int) optimum_N(Func<double, double> f, double a, double b, int N_min, int N_max, bool ave = false, bool CC=false){
        int best = 0;
        int lowest = (int)1e8;
        int N_f = 0; // Number of function calls.
        Func<double, double> f_counting = x => {N_f++; return f(x);};
        string toWrite = $"";
        string toWrite2 = $"";
        string method = "var";
        double res;
        for(int N = N_min; N <= N_max; N++){
            N_f = 0;
            N_ite = 0;
            if(ave && CC) {res = integrate_CCtransform(f_counting, a, b, N:N, ave:ave); method = "CC_ave";}
            else if(CC){res = integrate_CCtransform(f_counting, a, b, N:N); method = "CC";}
            else if(ave){res = integrate(f_counting, a, b, N:N, ave:true); method = "ave";}
            else res = integrate(f_counting, a, b, N:N);
            toWrite += $"{N}\t{N_f}\t";
            N_f /= N;
            toWrite += $"{N_f}\t{N_ite}\t{N_f/N_ite}\n";
            if(N_f < lowest) {
                lowest = N_f;
                best = N;
            }
            toWrite2 += $"{N}\t{res}\n";
        }
        File.WriteAllText("optimum_N_"+method+".data", toWrite);
        File.WriteAllText("vary_N_"+method+".data", toWrite2);
        return (lowest, best);
    }

    // Functions from integration homework.
    static double hw_CC_integrate(Func<double, double> f, double a, double b){
        return hw_integrate(x => f((a+b)/2+(b-a)/2*Cos(x))*Sin(x)*(b-a)/2, 0, PI);
    }
    static double hw_integrate (Func<double,double> f, double a, double b,
                                double delta=0.001, double eps=0.001, double f2=Double.NaN, double f3=Double.NaN){
        double h=b-a;
        if(Double.IsNaN(f2)){ f2=f(a+2*h/6); f3=f(a+4*h/6); } // first call, no points to reuse
        double f1=f(a+h/6), f4=f(a+5*h/6);
        double Q = (2*f1+f2+f3+2*f4)/6*(b-a); // higher order rule
        double q = (  f1+f2+f3+  f4)/4*(b-a); // lower order rule
        double err = Abs(Q-q);
        if (err <= delta+eps*Abs(Q)) return Q;
        else
            return (hw_integrate(f,a,(a+b)/2,delta/Sqrt(2),eps,f1,f2)+
                    hw_integrate(f,(a+b)/2,b,delta/Sqrt(2),eps,f3,f4));
    }
}