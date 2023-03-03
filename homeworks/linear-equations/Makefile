Out.txt : main.exe Makefile
	mono main.exe > Out.txt

main.exe : main.cs funcs.dll matrix.dll
#	mcs -target:exe -out:$@ $(filter %.cs,$^) -reference:./matlib/matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))
	
funcs.dll : funcs.cs
#	mcs -target:library -out:$@ *.cs -reference:./matlib/matrix.dll
#	mcs -target:library -out:$@ $(filter %.cs,$^) -reference:matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) -reference:matrix.dll

#matrix.dll : vector.cs matrix.cs
#    mcs -target:library -out:$@ $^

.PHONEY : clean

clean:
	$(RM) *.exe [Oo]ut* log*