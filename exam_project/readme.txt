Name: Jacob Christian Warming, 201905384@post.au.dk
Student number: 201905384 - 84 % 26 gives project 6.

Problem description:
— 6 —
    Adaptive 1D integrator with random nodes
    Implement our ordinary adaptive one-dimensional division-by-two integration algorithm where the abcissas at each iteration are chosen randomly (basically, the stratified sampling strategy applied to one-dimensional integral). Reuse points by passing the relevant statistics to the next level of iteration. For the error estimate you can use either the variance of your sample (as in plain monte-carlo), or simply the difference between the function average sampled with the new points and the function average inherited from the previous iteration.

    Find the optimum number N (the number of new points thrown at each iteration) by experimenting with your favourite integrals.

    You might need to use Clenshaw-Curtis transformation for hight-precision singular integrals as random abscissas probably provide a relatively slow convergence rate, if any.

    Compare with our predefined-nodes adaptive integrators. 
--------------------------------------------------------------------------------------------------------------

Most of the exercise is described in the Out.txt file. The rest is shown in various .svg files.

I have defined what corresponds to 6 (A), 9 (B), and 10 (C) points in the following way.
A is implementing the algorithm and testing that it works.
B is investigating the number of function calls depending on the number of new points thrown and arguing for an optimum number, N.
C is comparing with the homework implementation.
Adding to both B and C, the Clenshaw-Curtis transformation was implemented and tested in both cases.

When saying variance is used as error, I refer to the error being (b-a)*sigma/sqrt(N).
And when the error is the difference in mean, it is the absolute value of the mean from the curennt iteration minus the previous one, and multiplied by b-a.
The Clenshaw-Curtis transformation is referred to as CC-transformation (sometimes just CC) in the Out.txt file and .svg figures.

In my view, I have completed all 3 assignments, totalling 10 points for this exam project.