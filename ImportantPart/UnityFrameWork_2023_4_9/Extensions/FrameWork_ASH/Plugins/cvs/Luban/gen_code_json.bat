set WORKSPACE=..\..\..\..\..\..

set GEN_CLIENT=..\Luban\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=..\Luban\Configs

set OUTPUT_CODE_DIR=E:/game/FrameWork/Assets/Settings/Luban/Code
set OUTPUT_DATA_DIR=E:\game\FrameWork\Assets\Settings\Luban\Datas\Json

%GEN_CLIENT% -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_code_dir %OUTPUT_CODE_DIR% ^
 --output_data_dir %OUTPUT_DATA_DIR% ^
 --gen_types code_cs_unity_json,data_json ^
-s all 

pause