using System;
using System.IO;
using static System.Math;
using static System.Console;

class main{
    public static void Main(){
        //ys = (u', -u)
        Func<double, vector, vector> fs = (x, ys) => new vector(ys[1], -ys[0]);  //Moving the minus changes the phase by pi.
        vector yas = new vector(2,4);
        var (xs, ps) = driverA(fs, 0, yas, 6);
        string toWrite = $"";
        for(int i=0; i<xs.size; i++)
            toWrite += $"{xs[i]}\t{ps[i][0]}\t{ps[i][1]}\n";
        File.WriteAllText("test_harm_func.data", toWrite);

        //A4
        double b = 0.25;
        double c = 5.0;
        fs = (x,ys) => new vector(ys[1], -b*ys[1]-c*Sin(ys[0]));
        yas = new vector(PI-0.1, 0.0);
        (xs, ps) = driverA(fs, 0, yas, 10);
        toWrite = $"";
        for(int i=0; i<xs.size; i++)
            toWrite += $"{xs[i]}\t{ps[i][0]}\t{ps[i][1]}\n";
        File.WriteAllText("test_harm_func2.data", toWrite);

        //B
        vector y = driverB(fs, 0, yas, 10);
        WriteLine($"Final point according to A: {ps[ps.size-1][0]}\nAnd according to B: {y[0]}");
        y.print();
        // Lotka-Volterra
        double a = 1.5; b = 1; c = 3; double d = 1;
        fs = (x, ys) => new vector(a*ys[0] - b*ys[0]*ys[1], -c*ys[1] + d*ys[0]*ys[1]);
        yas = new vector(10, 5);
        var (xsA, psA) = driverA(fs, 0, yas, 15);
        var (xsB, psB) = driverB2(fs, 0, yas, 15, xlist: new genlist<double>(), ylist: new genlist<vector>());
        string toWriteA = $"";
        string toWriteB = $"";
        WriteLine($"{xsA.size}, {xsB.size}");
        for(int i=0; i<xsA.size; i++)
            toWriteA += $"{xsA[i]}\t{psA[i][0]}\t{psA[i][1]}\n";
        for(int i=0; i<xsB.size; i++)
            toWriteB += $"{xsB[i]}\t{psB[i][0]}\t{psB[i][1]}\n";
        File.WriteAllText("Lotka_A.data", toWriteA);
        File.WriteAllText("Lotka_B.data", toWriteB);

        //C
        double G = 1;
        double[] ms = {1, 1, 1};
        // ys = (x1, y1, x1', y1', x2, y2, x2', y2', x3, y3, x3', y3')
        //       0    1   2   3    4    5   6    7    8   9   10   11
        fs = delegate(double x, vector ys){
            var x1 = ys[0]; var y1 = ys[1]; var vx1 = ys[2]; var vy1 = ys[3];
            var x2 = ys[4]; var y2 = ys[5]; var vx2 = ys[6]; var vy2 = ys[7];
            var x3 = ys[8]; var y3 = ys[9]; var vx3 = ys[10]; var vy3 = ys[11];
            double m1 = ms[0]; double m2 = ms[1]; double m3 = ms[2];

            double r2_r1 = Pow((x2-x1)*(x2-x1) + (y2-y1)*(y2-y1),1.5);
            double r3_r1 = Pow((x3-x1)*(x3-x1) + (y3-y1)*(y3-y1),1.5);
            double r3_r2 = Pow((x3-x2)*(x3-x2) + (y3-y2)*(y3-y2),1.5);
            double ax1 = G*m2*(x2-x1)/r2_r1 + G*m3*(x3-x1)/r3_r1;
            double ay1 = G*m2*(y2-y1)/r2_r1 + G*m3*(y3-y1)/r3_r1;
            double ax2 = G*m1*(x1-x2)/r2_r1 + G*m3*(x3-x2)/r3_r2;
            double ay2 = G*m1*(y1-y2)/r2_r1 + G*m3*(y3-y2)/r3_r2;
            double ax3 = G*m1*(x1-x3)/r3_r1 + G*m2*(x2-x3)/r3_r2;
            double ay3 = G*m1*(y1-y3)/r3_r1 + G*m2*(y2-y3)/r3_r2;

            return new vector($"{vx1},{vy1},{ax1},{ay1},{vx2},{vy2},{ax2},{ay2},{vx3},{vy3},{ax3},{ay3}");
        };

        vector inits = new vector("0,0,-0.93240737, -0.86473146, -0.97000436, 0.24308753, 0.4662036850,"+
        "0.4323657300, 0.97000436, -0.24308753, 0.4662036850, 0.4323657300");
        var (xsC, psC) = driverB2(fs,0, inits, 6.3259, xlist: new genlist<double>(), ylist: new genlist<vector>());
        string C_write = $"";
        for(int i=0; i<xsC.size; i++){
            C_write += $"{xsC[i]}\t{psC[i][0]}\t{psC[i][1]}\t{psC[i][2]}\t{psC[i][3]}"+
            $"\t{psC[i][4]}\t{psC[i][5]}\t{psC[i][6]}\t{psC[i][7]}\t{psC[i][8]}\t{psC[i][9]}\t{psC[i][10]}\t{psC[i][11]}\n";
        }
        File.WriteAllText("3_body.data", C_write);
    }


    public static (vector,vector) rkstep12(
    Func<double,vector,vector> f, /* the f from dy/dx=f(x,y) */
    double x,                    /* the current value of the variable */
    vector y,                    /* the current value y(x) of the sought function */
    double h                     /* the step to be taken */){
        vector k0 = f(x,y);              /* embedded lower order formula (Euler) */
        vector k1 = f(x+h/2,y+k0*(h/2)); /* higher order formula (midpoint) */
        vector yh = y+k1*h;              /* y(x+h) estimate */
        vector er = (k1-k0)*h;           /* error estimate */
        return (yh,er);
    }

    public static (genlist<double>,genlist<vector>) driverA(
	Func<double,vector,vector> f, /* the f from dy/dx=f(x,y) */
	double a,                    /* the start-point a */
	vector ya,                   /* y(a) */
	double b,                    /* the end-point of the integration */
	double h=0.01,               /* initial step-size */
	double acc=0.01,             /* absolute accuracy goal */
	double eps=0.01              /* relative accuracy goal */){
        if(a>b) throw new ArgumentException("driver: a>b");
        double x=a; vector y=ya.copy();
        var xlist=new genlist<double>(); xlist.add(x);
        var ylist=new genlist<vector>(); ylist.add(y);
        do{
            if(x>=b) return (xlist,ylist); /* job done */
            if(x+h>b) h=b-x;               /* last step should end at b */
            var (yh,erv) = rkstep12(f,x,y,h);
            double tol = Max(acc,yh.norm()*eps) * Sqrt(h/(b-a));
            double err = erv.norm();
            if(err<=tol){ // accept step
                x+=h; y=yh;
                xlist.add(x);
                ylist.add(y);
            }
            h *= Min( Pow(tol/err,0.25)*0.95 , 2); // readjust stepsize
        }while(true);
    }//driver

    public static vector driverB(
	Func<double,vector,vector> f, /* the f from dy/dx=f(x,y) */
	double a,                    /* the start-point a */
	vector ya,                   /* y(a) */
	double b,                    /* the end-point of the integration */
	double h=0.01,               /* initial step-size */
	double acc=0.01,             /* absolute accuracy goal */
	double eps=0.01,              /* relative accuracy goal */
    genlist<double> xlist = null, genlist<vector> ylist = null){
        if(a>b) throw new ArgumentException("driver: a>b");
        double x=a; vector y=ya.copy();
        do{
            if(x>=b) return y; /* job done */
            if(x+h>b) h=b-x;               /* last step should end at b */
            var (yh,erv) = rkstep12(f,x,y,h);
            //double tol = Max(acc,yh.norm()*eps) * Sqrt(h/(b-a));
            //double err = erv.norm();
            //if(err<=tol) // accept step
            //    x+=h; y=yh;
            //h *= Min( Pow(tol/err,0.25)*0.95 , 2); // readjust stepsize
            //////////////////
            double[] tol = new double[y.size];
            for(int i=0;i<y.size;i++)
                tol[i] = Max(acc,Abs(yh[i])*eps)*Sqrt(h/(b-a));
            bool ok=true;
            for(int i=0;i<y.size;i++)
                if(!(erv[i]<tol[i]))
                    ok=false;
            if(ok)
                x+=h; y=yh;
            double factor = tol[0]/Abs(erv[0]);
            for(int i=1;i<y.size;i++)
                factor=Min(factor,tol[i]/Abs(erv[i]));
            h *= Min( Pow(factor,0.25)*0.95 ,2);
        }while(true);
    }//driver


    public static (genlist<double>,genlist<vector>) driverB2(
	Func<double,vector,vector> f, /* the f from dy/dx=f(x,y) */
	double a,                    /* the start-point a */
	vector ya,                   /* y(a) */
	double b,                    /* the end-point of the integration */
	double h=0.01,               /* initial step-size */
	double acc=0.01,             /* absolute accuracy goal */
	double eps=0.01,              /* relative accuracy goal */
    genlist<double> xlist = null, genlist<vector> ylist = null){
        if(a>b) throw new ArgumentException("driver: a>b");
        double x=a; vector y=ya.copy();
        if (xlist != null && ylist != null){
            xlist = new genlist<double>(); xlist.add(x);
            ylist = new genlist<vector>(); ylist.add(y);
        }
        do{
            if(x>=b){
                if (xlist == null && ylist == null){
                    xlist = new genlist<double>();
                    ylist = new genlist<vector>();
                    xlist.add(x);
                    ylist.add(y);
                    return (xlist, ylist);
                }
                else
                    return (xlist, ylist); /* job done */
            }
            if(x+h>b) h=b-x;               /* last step should end at b */
            var (yh,erv) = rkstep12(f,x,y,h);

            double[] tol = new double[y.size];
            double[] err = new double[y.size];
            for(int i=0;i<y.size;i++)
                tol[i] = Max(acc, yh.norm()*eps)*Sqrt(h/(b-a));
            bool ok=true;
            for(int i=0;i<y.size;i++){
                if(!(erv[i]<tol[i]))
                    ok=false;
            }
            if(ok){
                x+=h; y=yh;
                if(xlist != null && ylist != null)
                    xlist.add(x); ylist.add(y);
            }
            double factor = tol[0]/Abs(erv[0]);
            for(int i=1;i<y.size;i++)
                factor=Min(factor,tol[i]/Abs(erv[i]));
            h *= Min( Pow(factor,0.25)*0.95 ,2);
        }while(true);
    }//driver
}