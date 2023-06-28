Out.txt : main.exe Makefile input.txt input2.txt
	mono main.exe -numbers:1,2,3,4,5,6 -input:input2.txt -output:output.txt < input.txt > $@ 2> log

main.exe : main.cs cmath.dll
	mcs -reference:cmath.dll main.cs

cmath.dll : cmath.cs complex.cs
	mcs -target:library -out:cmath.dll $^

input.txt : Makefile
	echo 2 3 4 5 6 > input.txt

input2.txt : Makefile
	echo "3\n4\n5\n6\n7" > input2.txt

.PHONEY : clean
clean: Makefile
	$(RM) *.exe *.dll [Oo]ut* log*

# FROM LECTURE...
#all: Out.txt Out-numbers.txt

#Out.txt : main.exe Makefile# infile.txt
#	mono main.exe hello world -numbers:1,2,3,4,5,6\
		#-input:infile.txt \
		-output:outfile.txt\
		-numbers:1,2,3,4,5,6 1> $@# 2> log < infile.txt
	#cat infile.txt | mono main.exe hello world \
                -input:infile.txt \
                -output:outfile.txt\
                -numbers:1,2,3,4,5,6 1>> $@ 2>> log
	#cat infile.txt | mono main.exe hello world \
                -input:infile.txt \
                -output:outfile.txt\
                $$(cat numberx.txt) 1>> $@ 2>> log


#main.exe : main.cs input.dll cmath.dll
#	mcs -target:exe -out:$@ \
		$(filter %.cs,$^) $(addprefix -reference:, $(filter %.dll,$^))

#input.dll : input.cs
#	mcs -target:library -out:$@ $(filter %.cs,$^)

#numbers.txt : Makefile
#	echo "-numbers:7,8,9" > $@

#out-numbers.txt: numbers.txt
#	mono main.exe $$(cat $<) 1> numbers.txt

#infile.txt : Makefile
#	echo 1 >> $@
#	echo 2 >> $@
#	echo 3 >> $@

#DIR = $(HOME)/ppnm/matlib/complex          # or whatever your directory is
#cmath.dll : $(DIR)/cmath.cs $(DIR)/complex.cs
#cmath.dll : cmath.cs complex.cs
#	mcs -target:library -out:./cmath.dll $^

#.PHONEY : clean
#clean:
#	$(RM) *.exe *.dll [Oo]ut* log* #RM = rm ---force
#
#test:
#	echo $(RM)
