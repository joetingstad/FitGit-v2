package crc64eca6a01da5f22cce;


public class ViewPagerAdapter
	extends androidx.fragment.app.FragmentPagerAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getCount:()I:GetGetCountHandler\n" +
			"n_getItem:(I)Landroidx/fragment/app/Fragment;:GetGetItem_IHandler\n" +
			"";
		mono.android.Runtime.register ("fitgit.Adapters.ViewPagerAdapter, fitgit", ViewPagerAdapter.class, __md_methods);
	}


	public ViewPagerAdapter (androidx.fragment.app.FragmentManager p0)
	{
		super (p0);
		if (getClass () == ViewPagerAdapter.class)
			mono.android.TypeManager.Activate ("fitgit.Adapters.ViewPagerAdapter, fitgit", "AndroidX.Fragment.App.FragmentManager, Xamarin.AndroidX.Fragment", this, new java.lang.Object[] { p0 });
	}


	public ViewPagerAdapter (androidx.fragment.app.FragmentManager p0, int p1)
	{
		super (p0, p1);
		if (getClass () == ViewPagerAdapter.class)
			mono.android.TypeManager.Activate ("fitgit.Adapters.ViewPagerAdapter, fitgit", "AndroidX.Fragment.App.FragmentManager, Xamarin.AndroidX.Fragment:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1 });
	}


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();


	public androidx.fragment.app.Fragment getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native androidx.fragment.app.Fragment n_getItem (int p0);

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
