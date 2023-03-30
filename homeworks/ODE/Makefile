default : Out.txt test_A.svg A4_test.svg B_test.svg 3_body.svg 3_body.gif

Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs matrix.dll genlist.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

test_A.svg : test_A.gpi test_harm_func.data
	gnuplot $<

A4_test.svg : A4_test.gpi test_harm_func2.data
	gnuplot $<

B_test.svg : B_test.gpi Lotka_A.data Lotka_B.data
	gnuplot $<

3_body.svg : 3_body.gpi 3_body.data
	gnuplot $<

3_body.gif : 3_body_gif.gpi 3_body.data
	gnuplot $<

clean :
	$(RM) *.exe *.svg *.data *.gif