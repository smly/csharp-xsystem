TARGET_TEST_CLASS = xsystem.class.test.dll

SOURCES_TEST_CLASS = \
	Xsystem.UnitTests.Class/TestLittleEndianBitConverter.cs

$(TARGET_TEST_CLASS):
	mcs $(SOURCES_TEST_CLASS) \
	-r:nunit.framework.dll \
	-r:../src/xsystem.class.dll \
	-t:library \
	-out:$@

test: $(TARGET_TEST_CLASS)
	cp ../src/xsystem.class.dll ../tests/
	nunit-console \
	-nologo \
	-labels \
	$(TARGET_TEST_CLASS)

clean:
	rm -f $(TARGET_TEST_CLASS)
