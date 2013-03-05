package com.skiba.password;

import android.util.Base64;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;


public class clsGenerate {

	String b64pad  = "";
	int chrsz   = 8;
	
	public String generate(String secretpassword,String sitename,int pswlength) throws NoSuchAlgorithmException {
		String pwd = "";
		String error = "";
	    if (sitename.length() == 0)
	        error = error + " sitename";
	    if (secretpassword.length() == 0)
	        error = error + " secretpassword";
	    
	    if (error.length() == 0) {
	    	String input = secretpassword + ':' + sitename.toLowerCase();
	        pwd = binb2b64(core_sha1(input));
	        pwd = pwd.substring(0, pswlength);
	        pwd = ensurenumberandletter(pwd);
	    }
	    return pwd;
	}
	
	private byte[] core_sha1(String input) throws NoSuchAlgorithmException
	{
		MessageDigest md = MessageDigest.getInstance("SHA1");
		md.reset();
		byte[] buffer = input.getBytes();
		md.update(buffer);
		byte[] digest = md.digest();
		return digest;
	}	
	


	private String binb2b64(byte[] binarray)
	{
		return new String(Base64Coder.encode(binarray));
	}
	
	private String  ensurenumberandletter(String s) {
		String numbers = "123456789";
		String letters = "ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
		String punct = "?!#@&$";
	    int hasnumber = 0;
	    int hasletter = 0;
	    int huspunct = 0;

	    for (int i = 0; i < s.length(); i++) {
	        if (numbers.indexOf(s.charAt(i)) > -1)
	            hasnumber = 1;
	        if (letters.indexOf(s.charAt(i)) > -1)
	            hasletter = 1;
	        if (punct.indexOf(s.charAt(i)) > -1)
	            huspunct = 1;
	    }
	    if (hasnumber == 0)
	        s = "1" + s.substring(1);
	    if (hasletter == 0)
	        s = s.substring(0, 1) + "a" + s.substring(2);
	    if (huspunct == 0)
	        s = s.substring(0, 2) + "@" + s.substring(3);

	    return s;
	}	

}
