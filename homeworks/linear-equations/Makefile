default : Out.txt out_times.data times.svg
Out.txt : main.exe Makefile
	mono main.exe > Out.txt

times.svg : plot.gpi out_times.data
	gnuplot $<

main.exe : main.cs funcs.dll matrix.dll
#	mcs -target:exe -out:$@ $(filter %.cs,$^) -reference:./matlib/matrix.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))
	
funcs.dll : funcs.cs
#	mcs -target:library -out:$@ *.cs -reference:./matlib/matrix.dll
#	mcs -target:library -out:$@ $(filter %.cs,$^) -reference:matrix.dll
	mcs -target:library -out:$@ $(filter %.cs,$^) -reference:matrix.dll

out_times.data : main.exe
	>$@
	for N in $$(seq 10 10 200); do \
		time --format "$$N %e" --output $@ --append \
		mono $< -size:$$N 1>out 2>err ;\
	done

#matrix.dll : vector.cs matrix.cs
#    mcs -target:library -out:$@ $^

.PHONEY : clean

clean:
	$(RM) *.exe [Oo]ut* log* *.data