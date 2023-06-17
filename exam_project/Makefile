default : Out.txt

Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

clean :
	$(RM) *.exe *.svg *.data *.gif