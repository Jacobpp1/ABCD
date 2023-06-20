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
	}*/
	
	public static vector QRGSsolve(matrix Q, matrix R, vector b){
		vector x = new vector(R.size1);
		
		x = Q.transpose()*b;

		for (int i = x.size-1; i>=0; i--){
			double sum=0;
			for (int k=i+1; k<x.size; k++) sum += R[i,k] * x[k];
			x[i] = (x[i]-sum)/R[i,i];
		}
/*
		for(int i = b.size-1; i=>0; i--){
			double sum = 0;
			for(int k = i+1; k<b.size; k++) sum += R[i,k]*b[k];
			b[i] = (b[i]-sum)/R[i,i];
		}*/
		return x;
	}

	public static double QRGSdet(matrix A){
		double det = 1;
		matrix R = new matrix(A.size2, A.size2);
		QRGSdecomp(A,R);
		for(int i = 0; i<R.size2; i++)
			det *= R[i,i];
		return det;
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
}