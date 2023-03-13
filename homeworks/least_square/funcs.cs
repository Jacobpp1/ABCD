using System;
using static System.Math;
using static System.Console;
public class funcs{
    
    public static (vector, matrix) lsfit(Func<double, double>[] fs , vector x , vector y , vector dy ) {
        int n = x.size, m=fs.Length;
        var A = new matrix(n,m);
        var b = new vector(n);
        for ( int i=0; i<n; i++){
            b[i]=y[i]/dy[i];
            for(int k = 0; k<m; k++) A[i,k] = fs[k](x[i])/dy[i];
        }
        var qra = new GSQR(A);
        vector c = qra.solve(b);
        var pinvA = qra.pinverse();
        var S = pinvA*pinvA.T;
        return (c,S);
    }


    // From earlier exercises
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
	
	public static vector QRGSsolve(matrix Q, matrix R, vector b){
		vector x = new vector(R.size1);
		
		x = Q.transpose()*b;
        //Backsub in for loop.
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
/////////////////////////////////////////////////////////////////

    public static void timesJ(matrix A, int p, int q, double theta){
	    double c=Cos(theta),s=Sin(theta);
	    for(int i=0;i<A.size1;i++){
	    	double aip=A[i,p],aiq=A[i,q];
	    	A[i,p]=c*aip-s*aiq;
	    	A[i,q]=s*aip+c*aiq;
		}
    }

    public static void Jtimes(matrix A, int p, int q, double theta){
	    double c=Cos(theta),s=Sin(theta);
    	for(int j=0;j<A.size1;j++){
	    	double apj=A[p,j],aqj=A[q,j];
		    A[p,j]= c*apj+s*aqj;
		    A[q,j]=-s*apj+c*aqj;
		}
    }
    
    public static void cyclic(matrix A, matrix V){
        int n = A.size1;
        bool changed;
        do{
            changed=false;
            for(int p=0; p<n-1; p++){
                for(int q=p+1; q<n; q++){
                    double apq=A[p,q], app=A[p,p], aqq=A[q,q];
                    double theta=0.5*Atan2(2*apq,aqq-app);
                    double c=Cos(theta),s=Sin(theta);
                    double new_app=c*c*app-2*s*c*apq+s*s*aqq;
                    double new_aqq=s*s*app+2*s*c*apq+c*c*aqq;
                    if(new_app!=app || new_aqq!=aqq) // do rotation
                        {
                        changed=true;
                        timesJ(A,p,q, theta); // A←A*J 
                        Jtimes(A,p,q,-theta); // A←JT*A 
                        timesJ(V,p,q, theta); // V←V*J
                        }
                }
            }
        }
        while(changed);
    }

}