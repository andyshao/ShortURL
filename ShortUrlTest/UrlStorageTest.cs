﻿using Moq;

namespace ShortUrlTest
{
	using Xunit;
	using Nancy;
	using Nancy.Testing;
	using ShortUrl;

	public class UrlStorageTest
	{
		private Browser app;
		private Mock<UrlStore> fakeStorage;

		public UrlStorageTest()
		{
			//Given
			fakeStorage = new Moq.Mock<UrlStore>();
			ShortUrlModule artificiaReference;
			app = app = new Browser(
							new ConfigurableBootstrapper(
								with => with.Dependency(fakeStorage.Object)));

		}

		[Fact]
		public void app_should_store_url_when_one_is_posted()
		{
			//When
			app.Post("/",
					 with =>
					 {
						 with.FormValue("url", "http://www.longurlplease.com/");
						 with.HttpRequest();
					 });

			//Then
			fakeStorage
				.Verify(store =>
					store.SaveUrl("http://www.longurlplease.com/", Moq.It.IsAny<string>()));
		}

		[Fact]
		public void app_should_try_to_retrieve_url_when_getting_a_short_url()
		{
			//When
			app.Get("/shorturl",
			        with => with.HttpRequest());

			//Then
			fakeStorage
				.Verify(store => 
				store.GetUrlFor("shorturl"));
		}
	}
}