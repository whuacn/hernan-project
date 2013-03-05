package com.skiba.password;

import java.security.NoSuchAlgorithmException;

import android.os.Bundle;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.Context;
import android.view.Menu;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;

public class Psw extends Activity {


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_psw);
        
        Spinner cboTam = (Spinner)findViewById(R.id.cboSize);
        ArrayAdapter myAdap = (ArrayAdapter) cboTam.getAdapter(); //cast to an ArrayAdapter
        int spinnerPosition = myAdap.getPosition("10");
        cboTam.setSelection(spinnerPosition);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.activity_psw, menu);
        return true;
    }
    
    /** Called when the user clicks the Generate button 
     * @throws NoSuchAlgorithmException */
    @SuppressLint("NewApi")
	public void Generate(View view) throws NoSuchAlgorithmException {
       
    	EditText siteText = (EditText) findViewById(R.id.txtSite);
    	EditText pwsText = (EditText) findViewById(R.id.txtPassword);
    	Spinner cboTam = (Spinner)findViewById(R.id.cboSize);
    	
    	String site = siteText.getText().toString();
    	String pws = pwsText.getText().toString();    	
    	int tam = Integer.valueOf(cboTam.getSelectedItem().toString()); 	

    	
    	EditText ResultText = (EditText) findViewById(R.id.txtResult);
    	TextView ResultLabel = (TextView)findViewById(R.id.lbResult);
    	
    	if (site.trim() == "" || pws.trim() == "")
    	{
        	ResultLabel.setVisibility(View.INVISIBLE);
        	ResultText.setVisibility(View.INVISIBLE);
    		return;
    	}
    	
    	ResultLabel.setVisibility(View.VISIBLE);
    	ResultText.setVisibility(View.VISIBLE);
    	
    	clsGenerate g = new clsGenerate();
    	String password = g.generate(pws, site, tam);    
    	ResultLabel.setText("Password para " + site);
    	ResultText.setText(password);
    	
    	ClipData clip = ClipData.newPlainText("Password",password);
    	ClipboardManager clipboard = (ClipboardManager) getSystemService(Context.CLIPBOARD_SERVICE);
    	clipboard.setPrimaryClip(clip);
    	
    }
    
}
