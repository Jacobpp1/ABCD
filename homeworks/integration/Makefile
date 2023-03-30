default : Out.txt A.svg

Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

A.svg : A.gpi A.data
	gnuplot $<

clean :
	$(RM) *.exe *.svg *.data *.gif