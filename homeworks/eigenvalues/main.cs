using System;
using static System.Math;
using static System.Console;
public class main{
    public static void Main(){
        //A
        matrix A = new matrix("1,2,3,4; 2,3,4,5; 3,4,5,6; 4,5,6,7");
        matrix B = new matrix("10,11,12; 11,12,13; 12,13,14");
        matrix C = new matrix("15,16,17,18; 16,17,18,19; 17,18,19,20; 18,19,20,21");
        matrix V = C.copy();
        V = V.set_identity();
        
        //A1
        A.print();
        funcs.timesJ(A, 2, 3, 21.4);
        A.print();
        
        //A2
        B.print();
        funcs.Jtimes(B,2,2,21.4);
        B.print();
        
        //A3
        C.print();
        funcs.cyclic(C,V);
        C.print();
        V.print();
        matrix D = V.transpose()*V;
        D.print();


        //B Numerical
        /* mono main.exe -rmax:10 -dr:0.3 */
        /*
        int npoints = (int)(rmax/dr) - 1;
        vector r = new vector(npoints);
        for(int i=0;i<npoints;i++) r[i]=dr*(i+1);
        matrix H = new matrix(npoints,npoints);
        for(int i=0;i<npoints-1;i++){
            H[i,i]  =-2;
            H[i,i+1]= 1;
            H[i+1,i]= 1;
        }
        H[npoints-1,npoints-1]=-2;
        H.scale(-0.5/dr/dr);
        for(int i=0;i<npoints;i++) H[i,i]+=-1/r[i];
        */
    }
}