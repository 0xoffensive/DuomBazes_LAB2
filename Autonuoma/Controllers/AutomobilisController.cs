namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.Automobilis;


/// <summary>
/// Controller for working with 'Automobilis' entity.
/// </summary>
public class AutomobilisController : Controller
{
	/// <summary>
	/// This is invoked when either 'Index' action is requested or no action is provided.
	/// </summary>
	/// <returns>Entity list view.</returns>
	[HttpGet]
	public ActionResult Index()
	{
		return View(AutomobilisRepo.ListAutomobilis());
	}

	/// <summary>
	/// This is invoked when creation form is first opened in browser.
	/// </summary>
	/// <returns>Creation form view.</returns>
	[HttpGet]
	public ActionResult Create()
	{
		var autoCE = new AutomobilisCE();
		PopulateSelections(autoCE);

		return View(autoCE);
	}

	/// <summary>
	/// This is invoked when buttons are pressed in the creation form.
	/// </summary>
	/// <param name="autoCE">Entity model filled with latest data.</param>
	/// <returns>Returns creation from view or redirects back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Create(AutomobilisCE autoCE)
	{
		//form field validation passed?
		if( ModelState.IsValid )
		{
			AutomobilisRepo.InsertAutomobilis(autoCE);

			//save success, go back to the entity list
			return RedirectToAction("Index");
		}
		
		//form field validation failed, go back to the form
		PopulateSelections(autoCE);
		return View(autoCE);
	}

	/// <summary>
	/// This is invoked when editing form is first opened in browser.
	/// </summary>
	/// <param name="id">ID of the entity to edit.</param>
	/// <returns>Editing form view.</returns>
	[HttpGet]
	public ActionResult Edit(string vin)
	{
		var autoCE = AutomobilisRepo.FindAutomobolisCE(vin);
		PopulateSelections(autoCE);

		return View(autoCE);
	}

	/// <summary>
	/// This is invoked when buttons are pressed in the editing form.
	/// </summary>
	/// <param name="id">ID of the entity being edited</param>		
	/// <param name="autoCE">Entity model filled with latest data.</param>
	/// <returns>Returns editing from view or redirects back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Edit(string vin, AutomobilisCE autoCE)
	{
		//form field validation passed?
		if (ModelState.IsValid)
		{
			AutomobilisRepo.UpdateAutomobilis(autoCE);

			//save success, go back to the entity list
			return RedirectToAction("Index");
		}

		//form field validation failed, go back to the form
		PopulateSelections(autoCE);
		return View(autoCE);
	}

	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view.</returns>
	[HttpGet]
	public ActionResult Delete(string vin)
	{
		var autoEvm = AutomobilisRepo.FindAutomobolisCE(vin);
		return View(autoEvm);
	}

	/// <summary>
	/// This is invoked when deletion is confirmed in deletion form
	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view on error, redirects to Index on success.</returns>
	[HttpPost]
	public ActionResult DeleteConfirm(string vin)
	{
		//try deleting, this will fail if foreign key constraint fails
		try 
		{
			AutomobilisRepo.DeleteAutomobilis(vin);

			//deletion success, redired to list form
			return RedirectToAction("Index");
		}
		//entity in use, deletion not permitted
		catch( MySql.Data.MySqlClient.MySqlException )
		{
			//enable explanatory message and show delete form
			ViewData["deletionNotPermitted"] = true;

			var autoCE = AutomobilisRepo.FindAutomobolisCE(vin);
			PopulateSelections(autoCE);

			return View("Delete", autoCE);
		}
	}

	/// <summary>
	/// Populates select lists used to render drop down controls.
	/// </summary>
	/// <param name="autoCE">'Automobilis' view model to append to.</param>
	public void PopulateSelections(AutomobilisCE autoCE)
	{
		//build select lists
		var ListPavaruDezes = new List<string>{ "automatine", "mechanine" }; 
		var ListKebulai = new List<string>{
			"sedanas",
			"hecbekas",
			"universalas",
			"coupe",
			"SUV",
			"pikapas",
			"krosoveris",
			"kita"
		}; 
		var ListDegaluTipai = new List<string>{
			"benzinas",
			"dyzelinas",
			"elektra",
			"elektra-benzinas",
			"elektra-dyzelinas",
			"kita"
		};
		
		autoCE.Lists.PavaruDezes = ListPavaruDezes
			.Select(e => new SelectListItem
			{
				Value = Convert.ToString(e),
				Text = e.ToString()
			}).ToList();

		autoCE.Lists.KebuloTipai = ListKebulai
			.Select(e => new SelectListItem
			{
				Value = Convert.ToString(e),
				Text = e.ToString()
			}).ToList();

		autoCE.Lists.DegaluTipai = ListDegaluTipai
			.Select(e => new SelectListItem
			{
				Value = Convert.ToString(e),
				Text = e.ToString()
			}).ToList();
		

		//build select list for 'Modeliai'
		{
			//initialize the destination list
			autoCE.Lists.Modeliai = new List<SelectListItem>();

			//load 'Gamintojas' entities to use for item groups
			var gamintojai = GamintojasRepo.List();

			//create select list items from 'Modelis' related to each 'Gamintojas'
			foreach( var gamintojas in gamintojai )
			{
				//create list item group for current 'Gamintojas' entity
				var itemGrp = new SelectListGroup() { Name = gamintojas.Pavadinimas };

				//load related 'Modelis' entities
				var modeliai = ModelisRepo.ListForGamintojas(gamintojas.Id);

				//build list items for the group
				foreach( var modelis in modeliai )
				{
					var sle =
						new SelectListItem {
							Value = Convert.ToString(modelis.Id),
							Text = modelis.Pavadinimas,
							Group = itemGrp
						};
					autoCE.Lists.Modeliai.Add(sle);
				}
			}
		}
	}
}
