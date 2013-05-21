// JavaScript Document

//检查编辑区域范围内有多少字节
function CheckLength()
{
	MessageLength=document.form.Content.value.length;
	alert("您的内容已有 "+MessageLength+" 字节");
}

function PressKey(eventobject)
{
	if(event.ctrlKey && window.event.keyCode==13)
	{
		if(Check())  this.document.form.submit();
	}
}

function PressKey1(eventobject)
{
	if(event.ctrlKey && window.event.keyCode==13)
	{
		if(ReplyCheck())  this.document.form.submit();
	}
}

function Replac()
{
	txt2=prompt("查找内容","");
	if (txt2 != "") {
		txt=prompt("替换为：",txt2)
		}
	else 
	{
		Replac()
		}
	var Otext = txt2; var Itext = txt; 
	document.form.Content.value = eval('form.Content.value.replace(/'+Otext+'/'+'g'+',"'+Itext+'")')
}


function Copy(theField) 
{
	var tempval=eval("document."+theField)
	tempval.focus()
	tempval.select()
	therange=tempval.createTextRange()
	therange.execCommand("Copy")
}

//下拉分页
function MM_jumpMenu(targ,selObj,restore){ 		//v3.0
  eval(targ+".location='"+selObj.options[selObj.selectedIndex].value+"'");
  if (restore) selObj.selectedIndex=0;
}



/*****************************************************  
 *  函数名：	DateCheck()
 *  作  用：检查字符中是否在指定的大小范围内和是否含有非法字符
 *	参  数：date: 要检查的字符；
 *          minum:  字符最小的长度
 *  		maxnum: 字符最大的长度
 *  返回值：True:   数据不在指定范围内或含有非法字符
 *			False:  数据在指定范围内且不含有非法字符
 ******************************************************
*/
function DateCheck(date,minnum,maxnum)
{
	if (date.length < minnum || date.length > maxnum ){
		window.alert("输入数据的长度不能少于 " + minnum + " 位并且不能大于 " + maxnum + " 位!");
		return true;
	}

	 a = date.indexOf("'");
	 b = date.indexOf("|");
	 c = date.indexOf("\"");
	 
	 if (a != -1||b != -1 ||c != -1){
	   window.alert("您的输入含有特殊字符，请重新输入！");
	   return true;
	  	}
	return false;
}


/*****************************************************  
 *  函数名：	IsNum()
 *  作  用：检查字符是否是纯数字
 *	参  数：str: 要检查的字符；  
 *  返回值：True:   是数字
 *			False:  不是纯数字
 ******************************************************
*/
function IsNum(str)
{
	return !/\D/.test(str)
}


/*****************************************************  
 *  函数名：	IsStr()
 *  作  用：检查字符是否是字符
 *	参  数：str: 要检查的字符  
 *  返回值：True:   是字符
 *			False:  不是纯字符
 ******************************************************
*/
function IsStr(str)
{      
　　if (/[^\x00-\xff]/g.test(str))
	{
		return false;
	}
　　else 
	{
		return true;
	}
}


/*****************************************************  
 *  函数名：	IsEmail()
 *  作  用：检查Email地址是否合法
 *	参  数：date: 要检查的Email地址  
 *  返回值：True: Email 地址合法
 *          False:Email 地址不合法
 *****************************************************
*/
function IsEmail(vEMail)
{
	var regInvalid=/(@.*@)|(\.\.)|(@\.)|(\.@)|(^\.)/;
	var regValid=/^.+\@(\[?)[a-zA-Z0-9\-\.]+\.([a-zA-Z]{2,3}|[0-9]{1,3})(\]?)$/;
	return (!regInvalid.test(vEMail)&&regValid.test(vEMail));
}


/*****************************************************  
 *  函数名：	Check()
 *  作  用：提交留言表单数据验证
 *	参  数：无 
 *  返回值：True: 验证通过
 *          False: 验证没有通过
 ******************************************************
*/
function Check(){
    var check = false;
	var Name = document.form.Name.value;
	if ( Name =="") {
	    alert("你不会连大名也不输入吧!");
	    document.form.Name.focus();
	    return false;
	  }
	
	if(DateCheck(Name,1,16)){
		document.form.Name.focus();
		document.form.Name.value = "";
		return false;
	}
	
	var UserEmail = document.form.Mail.value;
	if(UserEmail!="")
	{
		if(!isEmail(UserEmail))
		{
			alert("请输入正确的电子邮件格式！");			
			document.form.Mail.focus();
			return false;
		}
	 }
	
	var QQ = document.form.QQ.value;
	if (QQ != "") {
		if(!IsNum(QQ))
		{
			alert("您输入的QQ不是纯数字,如果没有QQ请不要填写!");
			document.form.QQ.focus();
			document.form.QQ.value = "";
			return false;
		}
		if(DateCheck(QQ,4,10))
		{
			document.form.QQ.focus();
			document.form.QQ.value = "";
			return false;
		}
	}
		
	
	var ComeFrom = document.form.ComeFrom.value;
	if(ComeFrom != "")
	{	
		if(DateCheck(ComeFrom,2,50)){
			document.form.ComeFrom.focus();
			document.form.ComeFrom.value = "";
			return false;
		}
	}
		
	
	var HomePage = document.form.HomePage.value;
	if (HomePage!=""){
	   if(HomePage.indexOf("http://") == -1){
			alert("这不是有效的主页地址!");
			document.form.HomePage.focus();
			document.form.HomePage.value = "";
			return false;
		}
	}

	var Title = document.form.Title.value;
	if (Title=="")
	{
	   alert("请输入留言主题!");
	   document.form.Title.focus();
	   return false;
	 }
	
	if(DateCheck(Title,2,50))
	{
		document.form.Title.focus();
		document.form.Title.value = "";
		return false;
	}

	Content=document.form.Content.value;
	if (Content=="")
	{
	   alert("请输入留言内容!");
	   document.form.Content.focus();
	   return false;
	}
	
	if (document.form.Content.value.length < 2){
	   alert("留言的内容不会这么少吧!");
	   document.form.Content.focus();
	   return false;
	}
  	return true;
}


/*****************************************************  
 *  函数名：	ReplyCheck()
 *  作  用：回复留言表单数据验证
 *	参  数：无 
 *  返回值：True: 验证通过
 *          False: 验证没有通过
 *****************************************************
*/
function ReplyCheck(){
    var check = false;
	if (document.form.Content.value==""){
	   alert("输入数据的长度不能为空!");
	   document.form.Content.focus();
	   return false;
	}
	
	if (document.form.Content.value.length < 2){
	   alert("输入数据的长度不能少于2位!");
	   document.form.Content.focus();
	   return false;
	}
  return true;
}


function go(src,q)
{
	var ret;
	ret = confirm(q);
	if(ret!=false)window.location=src;
}



/*****************************************************  
 *  函数名：	ModPwdCheck()
 *  作  用：修改密码表单数据验证
 *	参  数：无 
 *  返回值：True: 验证通过
 *          False: 验证没有通过
 *****************************************************
*/
function ModPwdCheck()
{
	var ModPwdcheck = false;
	var User = document.form.User.value;
	if (User == "") {
	alert("请输入你的用户名!");
	document.form.User.focus();
		return false;
	}
	
	if(DateCheck(User,4,16)){
		document.form.User.focus();
		document.form.User.value = "";
		return false;
	}
	
	var Pwd = document.form.Pwd.value;
	if (Pwd == "") {
	alert("请输入你的旧密码!");
	document.form.Pwd.focus();
		return false;
	}
	
	if(DateCheck(Pwd,4,16)){
		document.form.Pwd.focus();
		document.form.Pwd.value = "";
		return false;
	}
	
	var NewPwd = document.form.NewPwd.value;
	if (NewPwd == "") {
	alert("新密码不能为空！");
	document.form.NewPwd.focus();	
		return false;
	} 
	
	if(DateCheck(NewPwd,4,16)){
		document.form.NewPwd.focus();
		document.form.NewPwd.value = "";
		return false;
	}
	
	var NewPwd2 = document.form.NewPwd2.value; 
	if (NewPwd2 == "") {
	alert("请确认新密码！");
	document.form.NewPwd2.focus();		
		return false;
      }
	  
	if(DateCheck(NewPwd2,4,16)){
		document.form.NewPwd2.focus();
		document.form.NewPwd2.value = "";
		return false;
	}
	 
	if (NewPwd != NewPwd2) {
	alert("你两次输入的密码不一致，请重输入！");
	document.form.NewPwd.focus();	
	document.form.NewPwd.value = "";
	document.form.NewPwd2.value = "";
		return false;
	}
	 
  return true;
  }    
 
 
/*****************************************************  
 *  函数名：	LoginCheck()
 *  作  用：登录表单数据验证
 *	参  数：无 
 *  返回值：True: 验证通过
 *          False: 验证没有通过
 *****************************************************
*/
 function LoginCheck()
 {
	var LoginCheck = false;
	var User = document.form.User.value;
	if (User == "") {
	alert("请输入你的用户名!");
	document.form.User.focus();
		return false;
	}
	
	if(DateCheck(User,4,16)){
		document.form.User.focus();
		document.form.User.value = "";
		return false;
	}
	
	var Pwd = document.form.Pwd.value;
	if (Pwd == "") {
	alert("请输入你的密码!");
	document.form.Pwd.focus();
		return false;
	}
	
	if(DateCheck(Pwd,4,16)){
		document.form.Pwd.focus();
		document.form.Pwd.value = "";
		return false;
	}
	
	var VerifyCode = document.form.VerifyCode.value;
	if (VerifyCode == "" || VerifyCode.length != 4) {
	alert("请输入正确的验证码!");
	document.form.VerifyCode.focus();
		return false;
	}
  return true;
  }  
  

/*****************************************************  
 *  函数名：	AdminInfoCheck()
 *  作  用：管理员信息表单数据验证
 *	参  数：无 
 *  返回值：True: 验证通过
 *          False: 验证没有通过
 *****************************************************
*/
function AdminInfoCheck()
 {
	var AdminInfoCheck = false;
	var AdminName = document.form.AdminName.value;
	if (AdminName == "") {
	alert("请输入管理员名称!");
	document.form.AdminName.focus();
		return false;
	}
	
	if(DateCheck(AdminName,4,16)){
		document.form.AdminName.focus();
		document.form.AdminName.value = "";
		return false;
	}
	
	var AdminUid = document.form.AdminUid.value;
	if (AdminUid == "") {
	alert("请输入管理员留言ID!");
	document.form.AdminUid.focus();
		return false;
	}
	
	if(DateCheck(AdminUid,4,16)){
		document.form.AdminUid.focus();
		document.form.AdminUid.value = "";
		return false;
	}
	
	var AdminMail = document.form.AdminMail.value;
	if(AdminMail =="")
	{
		alert("请输入电子邮箱！");
		document.form.AdminMail.focus();
		document.form.AdminMail.value = "";
		return false;
	}
		
	if(!IsEmail(AdminMail))
	{
		alert("请输入正确的电子邮箱！");
		document.form.AdminMail.focus();
		document.form.AdminMail.value = "";
		return false;
	}
	
	var AdminQQ = document.form.AdminQQ.value;
	if(AdminQQ =="")
	{
		alert("请输入您的QQ！");
		document.form.AdminQQ.focus();
		document.form.AdminQQ.value = "";
		return false;
	}
		
	if (DateCheck(AdminQQ,4,10))
	{
		document.form.AdminQQ.focus();
		document.form.AdminQQ.value = "";
		return false;
	}
	
	if (!IsNum(AdminQQ))
	{
		alert("输入的QQ必需为数字！");
		document.form.AdminQQ.focus();
		document.form.AdminQQ.value = "";
		return false;
	}

  return true;
  }  
 
 
/*****************************************************  
 *  函数名：	AdminConfigCheck()
 *  作  用：参数设置表单数据验证
 *	参  数：无 
 *  返回值：True: 验证通过
 *          False: 验证没有通过
 *****************************************************
*/ 
function AdminConfigCheck()
 {
	var AdminConfigCheck = false;
	var LybTitle = document.form.LybTitle.value;
	if (LybTitle == "") {
	alert("请输入留言本名称!");
	document.form.LybTitle.focus();
		return false;
	}
	
	if(DateCheck(LybTitle,4,20)){
		document.form.LybTitle.focus();
		document.form.LybTitle.value = "";
		return false;
	}
	
	var LybPageSize = document.form.LybPageSize.value;
	if (LybPageSize == "") {
		alert("请输入每页显示的留言数!");
		document.form.LybPageSize.focus();
		return false;
	}
	
	if(DateCheck(LybPageSize,1,2)){
		document.form.LybPageSize.focus();
		document.form.LybPageSize.value = "";
		return false;
	}
	
	if(!IsNum(LybPageSize))
	{
		alert("每页显示的留言数必需为数字！");
		document.form.LybPageSize.focus();
		document.form.LybPageSize.value = "";
		return false;
	}
	
	var LybMaxByte = document.form.LybMaxByte.value;
	if(LybMaxByte =="")
	{
		alert("请输入留言的最大字符数！");
		document.form.LybMaxByte.focus();
		document.form.LybMaxByte.value = "";
		return false;
	}
	
	if(DateCheck(LybMaxByte,3,5)){
		document.form.LybMaxByte.focus();
		document.form.LybMaxByte.value = "";
		return false;
	}
	
	if(!IsNum(LybMaxByte))
	{
		alert("留言的最大字符数必需为数字！");
		document.form.LybMaxByte.focus();
		document.form.LybMaxByte.value = "";
		return false;
	}
  return true;
  }  
  
  
  

