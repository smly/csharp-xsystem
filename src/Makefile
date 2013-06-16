TARGET_CORE = xsystem3.exe
TARGET_CLASS = xsystem.class.dll

SOURCES_CORE = \
	Xsystem.Application/Core.cs

SOURCES_CLASS = \
	Xsystem.Class/LittleEndianBitConverter.cs \
	Xsystem.Class/DriLoader.cs \
	Xsystem.Class/CgLoader.cs \
	Xsystem.Class/VspFormat.cs

$(TARGET_CORE): $(TARGET_CLASS)
	mcs $(SOURCES_CORE) \
	-r:./xsystem.class.dll \
	-out:$@

$(TARGET_CLASS):
	mcs $(SOURCES_CLASS) \
	-out:$@ \
	-t:library

all: $(TARGET_CORE)

clean:
	rm -f $(TARGET_CORE)
	rm -f $(TARGET_CLASS)
