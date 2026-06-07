%########## MATLAB R2016a Sample ##########################################
% if loadlibrary error, please install mingw.
%setenv('MW_MINGW64_LOC','C:\mingw-64');

% SLMFunc.dll & FTD3XX.dll. if 32bit MATLAB change files.
[notfound, warnings] = loadlibrary('SLMFunc','SLMFunc_MAT.h');

fprintf('sample start\n');

DisplayNumber = 2;
SLMNumber = 1;

var=0;
c = blanks(128);
s1 = libpointer('uint16Ptr', var);
s2 = libpointer('uint16Ptr', var);
d1 = libpointer('uint32Ptr', var);
d2 = libpointer('uint32Ptr', var);
c1 = libpointer('cstring',c);
c2 = libpointer('cstring',c);

%########## DVI test ######################################################
[Status, width,height,name] = calllib('SLMFunc','SLM_Disp_Info2', DisplayNumber,s1,s2,c1);
fprintf('ret= %d, width = %d, height = %d, Name %s\n', Status, s1.Value,s2.Value,c1.Value);
fprintf('ret= %d, width = %d, height = %d, Name %s\n', Status, width,height,name);

if 1
    fprintf('DVI Open\n');
    [ret] = calllib('SLMFunc','SLM_Disp_Open', DisplayNumber);
    fprintf('open ret= %d\n', ret);
    if(ret ~= 0) 
        fprintf('cannot open\n');
        return;
    end

    y = calllib('SLMFunc','SLM_Disp_GrayScale', DisplayNumber, 0, 500);
    drawnow
    pause(1);

    data = zeros(1920,1200,'uint16');

    data(100:300,100:300) = 100;
    data(301:500,301:500) = 200;
    data(501:700,501:700) = 300;
    data(701:900,701:900) = 400;

    d = libpointer('uint16Ptr', data);

    [ret] = calllib('SLMFunc','SLM_Disp_Data', DisplayNumber, 1920, 1200, 0, d);
    drawnow
    pause(1);
    str = 'C:\santec\SLM-200\Files\santec_logo.csv';
    str1 = libpointer('cstring', str);

    [ret] = calllib('SLMFunc','SLM_Disp_ReadCSV_A',DisplayNumber,0,str1);
    drawnow
    pause(1);
    
    fprintf('DVI Close\n');

    [ret] = calllib('SLMFunc','SLM_Disp_Close', DisplayNumber);
end


%########## Memory mode Test ###############################################
if 1
    %# If it's already open. Open ret = -10003
    %[ret] = calllib('SLMFunc','SLM_Ctrl_Close', SLMNumber);
    
    %###### USB Open ###########
    [ret] = calllib('SLMFunc','SLM_Ctrl_Open', SLMNumber);
    fprintf('open ret= %d\n', ret);
    if(ret ~= 0) 
        fprintf('cannot open\n');
        return;
    end

    %# ChangeMode ####################
    % 0:Memory mode
    % 1:DIV mode
    %#################################
    mode = 0;
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteVI', SLMNumber, mode);

    ret_status = -1;
    pause(0.1);

    %###### ReadVI ###########
    for ct = 1:5
        [ret,mode] = calllib('SLMFunc','SLM_Ctrl_ReadVI', SLMNumber, d1);
        if  ret == 0
            break;
        else
            fprintf('ReadVI Retry %d %d\n', ret_status,ct);
            pause(0.5);
        end
    end
    fprintf('ReadVI %d\n', mode);

    if 0
        %###### WriteWL & WriteAW ###########
        [ret] = calllib('SLMFunc','SLM_Ctrl_WriteWL', SLMNumber, 650, 200);
        [ret] = calllib('SLMFunc','SLM_Ctrl_WriteAW', SLMNumber);
    end

    %###### ReadWL ###########
    [ret,wevelength,phase] = calllib('SLMFunc','SLM_Ctrl_ReadWL', SLMNumber, d1, d2);
    fprintf('ret:%d,wavelength:%d,phase:%0.2f\n', ret,wevelength, double(phase)/100);

    %###### WriteGS ###########
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteGS', SLMNumber, 0);

    %###### WriteMI CSV ###########
    str = 'C:\santec\SLM-200\Files\santec_logo.csv';
    str1 = libpointer('cstring', str);
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteMI_CSV_A', SLMNumber, 1,0,str1);

    str = 'C:\santec\SLM-200\Files\Laguerre-Gaussian(LG0,-1).csv';
    str1 = libpointer('cstring', str);
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteMI_CSV_A', SLMNumber, 2,0,str1);
    
    str = 'C:\santec\SLM-200\Files\santec_mark.csv';
    str1 = libpointer('cstring', str);
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteMI_CSV_A', SLMNumber, 3,0,str1);
    
    %###### change interval ###########
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteMW', SLMNumber, 30);

    %###### start ###########
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteDR', SLMNumber, 0);

try
    for ct = 1:5
        [ret] = calllib('SLMFunc','SLM_Ctrl_ReadT',SLMNumber,d1,d2);
        fprintf('ret:%d,device:%0.2f,option:%0.2f\n',ret, double(d1.Value)/10,double(d2.Value)/10);
        [ret,dE,oE] = calllib('SLMFunc','SLM_Ctrl_ReadEDO',SLMNumber,d1,d2);
        fprintf('ret:%d,device error:%x,option error:%x\n',ret, d1.Value, d2.Value);
        [ret,dID,oID] = calllib('SLMFunc','SLM_Ctrl_ReadSDO',SLMNumber,c1,c2);
        fprintf('ret:%d,device id:%s,option id:%s\n',ret, dID, oID);
        pause(1)
    end
catch ME
   rethrow(ME)
end     

    %###### stop ###########
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteDB', SLMNumber);

    %###### USB Close ###########
    [ret] = calllib('SLMFunc','SLM_Ctrl_Close', SLMNumber);
    fprintf('close ret = %d\n', ret);


    %###### DVI mode ###########
    mode = 1;
    fprintf('DVI mode. Please wait.\n');
    tic;  % 計測スタート
    [ret] = calllib('SLMFunc','SLM_Ctrl_WriteVI', SLMNumber, mode);
    toc;  % 計測終了(計測スタートからの時間を表示)    
    fprintf('Done.\n');


end

unloadlibrary('SLMFunc');
fprintf('sample end\n');


