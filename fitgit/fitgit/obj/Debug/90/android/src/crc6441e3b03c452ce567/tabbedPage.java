package crc6441e3b03c452ce567;


public class tabbedPage
	extends androidx.fragment.app.FragmentActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"";
		mono.android.Runtime.register ("fitgit.tabbedPage, fitgit", tabbedPage.class, __md_methods);
	}


	public tabbedPage ()
	{
		super ();
		if (getClass () == tabbedPage.class)
			mono.android.TypeManager.Activate ("fitgit.tabbedPage, fitgit", "", this, new java.lang.Object[] {  });
	}


	public tabbedPage (int p0)
	{
		super (p0);
		if (getClass () == tabbedPage.class)
			mono.android.TypeManager.Activate ("fitgit.tabbedPage, fitgit", "System.Int32, mscorlib", this, new java.lang.Object[] { p0 });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
