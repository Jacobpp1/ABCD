Out.txt : main.exe Makefile input.txt input2.txt
	mono main.exe -numbers:1,2,3,4,5,6 -input:input2.txt -output:output.txt < input.txt > $@ 2> log

main.exe : main.cs input.dll 
	mcs -target:exe -out:$@ \
		$(filter %.cs,$^) $(addprefix -reference:, $(filter %.dll,$^))

input.dll : input.cs
	mcs -target:library -out:$@ $(filter %.cs,$^)

numbers.txt : Makefile
	echo "-numbers:7,8,9" > $@

infile.txt : Makefile
	echo 1 >> $@
	echo 2 >> $@
	echo 3 >> $@

.PHONEY : clean
clean: 
	$(RM) *.exe *.dll [Oo]ut* log*

test:
	echo $(RM)