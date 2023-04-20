using System;
using static System.Math;
public static partial class funcs{
	
	public static void QRGSdecomp(matrix A, matrix R){
		for(int i=0; i<R.size1; i++){
			R[i,i] = A[i].norm();
			A[i] /= R[i,i];
			for(int j=i+1; j<R.size1; j++){
				R[i,j] = A[i].dot(A[j]);
				A[j] -= A[i]*R[i,j];
			}
		}
	}

/*	public static void backsub ( matrix U, vector c) {
		for (int i=c.size -1; i >=0; i--){
			double sum=0;
			for (int k=i +1; k<c.size ; k++) sum += U[i,k] * c[k];
			c[i]=(c[i]-sum)/U[i,i]; 
			
		}
	}
*/	
	public static vector QRGSsolve(matrix Q, matrix R, vector b){
		vector x = new vector(R.size1);
		
		x = Q.transpose()*b;

		for (int i=x.size-1; i>=0; i--){
			double sum=0;
			for (int k=i+1; k<x.size; k++) sum += R[i,k] * x[k];
			x[i] = (x[i]-sum)/R[i,i];
		}

		return x;
	}

	public static matrix QRGSinverse(matrix Q, matrix R){
		matrix outMat = new matrix(Q.size1, Q.size2);
		for (int i=0; i<Q.size2; i++){
			vector e = new vector(Q.size2);
			e[i] = 1;
			outMat[i] = QRGSsolve(Q,R,e);
		}
		return outMat;
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