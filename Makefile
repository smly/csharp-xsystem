all: xsystem3.exe

xsystem3.exe:
	cd src; make

clean:
	cd src; make clean

test: xsystem3.exe
	cd tests; make test
