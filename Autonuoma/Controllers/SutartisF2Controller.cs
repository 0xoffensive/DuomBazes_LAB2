namespace Org.Ktu.Isk.P175B602.Autonuoma.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Newtonsoft.Json;

using Org.Ktu.Isk.P175B602.Autonuoma.Repositories;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.SutartisF2;
using Org.Ktu.Isk.P175B602.Autonuoma.Models;
using Org.Ktu.Isk.P175B602.Autonuoma.Models.ContractsReport;


/// <summary>
/// Controller for working with 'Sutartis' entity. Implementation of F2 version.
/// </summary>
public class SutartisF2Controller : Controller
{
	/// <summary>
	/// This is invoked when either 'Index' action is requested or no action is provided.
	/// </summary>
	/// <returns>Entity list view.</returns>
	[HttpGet]
	public ActionResult Index()
	{
		return View(SutartisF2Repo.ListSutartis());
	}

	/// <summary>
	/// This is invoked when creation form is first opened in a browser.
	/// </summary>
	/// <returns>Entity creation form.</returns>
	[HttpGet]
	public ActionResult Create()
	{
		var sutCE = new SutartisCE();

		sutCE.Sutartis.PriemimoData = DateTime.Now;
		//sutCE.Sutartis.PlanuojamaSuremontuotiData = DateTime.Now;
		
		PopulateLists(sutCE);

		return View(sutCE);
	}


	/// <summary>
	/// This is invoked when buttons are pressed in the creation form.
	/// </summary>
	/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
	/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
	/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
	/// <param name="sutCE">Entity view model filled with latest data.</param>
	/// <returns>Returns creation from view or redirets back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Create(int? save, int? addMechanikas, int? removeMechanikas,
										  int? addGedimas, int? removeGedimas,
										  int? addDarbas, int? removeDarbas,
										  SutartisCE sutCE)
	{
		//addition of new record was requested?
		if( addMechanikas != null )
		{
			//add entry for the new record
			var up =
				new SutartisCE.PrirasytasMech {
					InListId = sutCE.PriklausantysMechanikai.Count > 0 ? sutCE.PriklausantysMechanikai.Max(it => it.InListId) + 1 : 0,
					Mechanikas = null
				};
			sutCE.PriklausantysMechanikai.Add(up);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(sutCE);
			return View(sutCE);
		}

		if( addGedimas != null )
		{
			var up =
				new SutartisCE.Gedimas {
					InListId = sutCE.Gedimai.Count > 0 ? sutCE.Gedimai.Max(it => it.InListId) + 1 : 0,
					GedimoId = null
				};
			sutCE.Gedimai.Add(up);

			ModelState.Clear();

			PopulateLists(sutCE);
			return View(sutCE);
		}

		if( addDarbas != null )
		{
			var up =
				new SutartisCE.AtlDarb {
					InListId = sutCE.AtliktiDarbai.Count > 0 ? sutCE.AtliktiDarbai.Max(it => it.InListId) + 1 : 0,
					DarboPav = null,
					DarbKaina = null,
					Nuvaziuota = null,
					DetaleId = null,
					DetaleKiekis = 0
				};
			sutCE.AtliktiDarbai.Add(up);

			ModelState.Clear();

			PopulateLists(sutCE);
			return View(sutCE);
		}

		//removal of existing 'UzsakytosPaslaugos' record was requested?
		if( removeMechanikas != null )
		{
			//filter out 'Mechanikas' record having in-list-id the same as the given one
			sutCE.PriklausantysMechanikai =
				sutCE
					.PriklausantysMechanikai
					.Where(it => it.InListId != removeMechanikas.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(sutCE);
			return View(sutCE);
		}

		if( removeGedimas != null )
		{
			sutCE.Gedimai =
				sutCE
					.Gedimai
					.Where(it => it.InListId != removeGedimas.Value)
					.ToList();

			ModelState.Clear();

			PopulateLists(sutCE);
			return View(sutCE);
		}

		if( removeDarbas != null )
		{
			sutCE.AtliktiDarbai =
				sutCE
					.AtliktiDarbai
					.Where(it => it.InListId != removeDarbas.Value)
					.ToList();

			ModelState.Clear();

			PopulateLists(sutCE);
			return View(sutCE);
		}

		//save of the form data was requested?
		if( save != null )
		{
			for( var i = 0; i < sutCE.PriklausantysMechanikai.Count-1; i ++ )
			{
				var refUp = sutCE.PriklausantysMechanikai[i];

				for( var j = i+1; j < sutCE.PriklausantysMechanikai.Count; j++ )
				{
					var testUp = sutCE.PriklausantysMechanikai[j];
					
					if( testUp.Mechanikas == refUp.Mechanikas )
						ModelState.AddModelError($"PriklausantysMechanikai[{j}].Mechanikas", "Mechaniko dublikatas.");
				}
			}

			for( var i = 0; i < sutCE.Gedimai.Count-1; i ++ )
			{
				var refUp = sutCE.Gedimai[i];

				for( var j = i+1; j < sutCE.Gedimai.Count; j++ )
				{
					var testUp = sutCE.Gedimai[j];
					
					if( testUp.GedimoId == refUp.GedimoId )
						ModelState.AddModelError($"Gedimai[{j}].GedimoId", "Gedimo dublikatas.");
				}
			}

			//form field validation passed?
			if( ModelState.IsValid )
			{
				//create new 'Sutartis'
				sutCE.Sutartis.Nr = SutartisF2Repo.InsertSutartis(sutCE);

				foreach (var mech in sutCE.PriklausantysMechanikai)
					SutartisF2Repo.InsertAptarnauja(sutCE.Sutartis.Nr, mech.Mechanikas);

				foreach (var ged in sutCE.Gedimai)
					SutartisF2Repo.InsertTuriGedima(sutCE.Sutartis.Nr, ged.GedimoId);

				//create new 'AtliktiDarbai' records
				foreach( var upVm in sutCE.AtliktiDarbai )
				{
					int AtliktasDarbasId = SutartisF2Repo.InsertAtliktasDarbas(sutCE.Sutartis.Nr, upVm);
					SutartisF2Repo.InsertDetalesKiekis(AtliktasDarbasId, upVm);
				}

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			else
			{
				PopulateLists(sutCE);
				return View(sutCE);
			}
		}

		//should not reach here
		throw new Exception("Should not reach here.");
	}

	/// <summary>
	/// This is invoked when editing form is first opened in browser.
	/// </summary>
	/// <param name="id">ID of the entity to edit.</param>
	/// <returns>Editing form view.</returns>
	[HttpGet]
	public ActionResult Edit(int id)
	{
		var sutCE = SutartisF2Repo.FindSutartisCE(id);
		
		sutCE.PriklausantysMechanikai = SutartisF2Repo.ListAptarnauja(id);
		sutCE.Gedimai = SutartisF2Repo.ListTuriGedima(id);
		sutCE.AtliktiDarbai = SutartisF2Repo.ListAtliktasDarbas(id);
		PopulateLists(sutCE);

		return View(sutCE);
	}

	/// <summary>
	/// This is invoked when buttons are pressed in the editing form.
	/// </summary>
	/// <param name="id">ID of the entity being edited</param>
	/// <param name="save">If not null, indicates that 'Save' button was clicked.</param>
	/// <param name="add">If not null, indicates that 'Add' button was clicked.</param>
	/// <param name="remove">If not null, indicates that 'Remove' button was clicked and contains in-list-id of the item to remove.</param>
	/// <param name="sutCE">Entity view model filled with latest data.</param>
	/// <returns>Returns editing from view or redired back to Index if save is successfull.</returns>
	[HttpPost]
	public ActionResult Edit(int id, int? save, int? addMechanikas, int? removeMechanikas,
										  		int? addGedimas, int? removeGedimas,
										 		int? addDarbas, int? removeDarbas,
												SutartisCE sutCE)
	{
		if( addMechanikas != null )
		{
			//add entry for the new record
			var up =
				new SutartisCE.PrirasytasMech {
					InListId = sutCE.PriklausantysMechanikai.Count > 0 ? sutCE.PriklausantysMechanikai.Max(it => it.InListId) + 1 : 0,
					Mechanikas = null
				};
			sutCE.PriklausantysMechanikai.Add(up);

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(sutCE);
			return View(sutCE);
		}

		if( addGedimas != null )
		{
			var up =
				new SutartisCE.Gedimas {
					InListId = sutCE.Gedimai.Count > 0 ? sutCE.Gedimai.Max(it => it.InListId) + 1 : 0,
					GedimoId = null
				};
			sutCE.Gedimai.Add(up);

			ModelState.Clear();

			PopulateLists(sutCE);
			return View(sutCE);
		}

		if( addDarbas != null )
		{
			var up =
				new SutartisCE.AtlDarb {
					InListId = sutCE.AtliktiDarbai.Count > 0 ? sutCE.AtliktiDarbai.Max(it => it.InListId) + 1 : 0,
					DarboPav = null,
					DarbKaina = null,
					Nuvaziuota = null,
					DetaleId = null,
					DetaleKiekis = 0
				};
			sutCE.AtliktiDarbai.Add(up);

			ModelState.Clear();

			PopulateLists(sutCE);
			return View(sutCE);
		}

		//removal of existing 'UzsakytosPaslaugos' record was requested?
		if( removeMechanikas != null )
		{
			//filter out 'Mechanikas' record having in-list-id the same as the given one
			sutCE.PriklausantysMechanikai =
				sutCE
					.PriklausantysMechanikai
					.Where(it => it.InListId != removeMechanikas.Value)
					.ToList();

			//make sure @Html helper is not reusing old model state containing the old list
			ModelState.Clear();

			//go back to the form
			PopulateLists(sutCE);
			return View(sutCE);
		}

		if( removeGedimas != null )
		{
			sutCE.Gedimai =
				sutCE
					.Gedimai
					.Where(it => it.InListId != removeGedimas.Value)
					.ToList();

			ModelState.Clear();

			PopulateLists(sutCE);
			return View(sutCE);
		}

		if( removeDarbas != null )
		{
			sutCE.AtliktiDarbai =
				sutCE
					.AtliktiDarbai
					.Where(it => it.InListId != removeDarbas.Value)
					.ToList();

			ModelState.Clear();

			PopulateLists(sutCE);
			return View(sutCE);
		}

		//save of the form data was requested?
		if( save != null )
		{
			// tikrina mechanikus
			for( var i = 0; i < sutCE.PriklausantysMechanikai.Count-1; i ++ )
			{
				var refUp = sutCE.PriklausantysMechanikai[i];

				for( var j = i+1; j < sutCE.PriklausantysMechanikai.Count; j++ )
				{
					var testUp = sutCE.PriklausantysMechanikai[j];
					
					if( testUp.Mechanikas == refUp.Mechanikas )
						ModelState.AddModelError($"PriklausantysMechanikas[{j}].Mechanikai", "Mechaniko dublikatas.");
				}
			}

			for( var i = 0; i < sutCE.Gedimai.Count-1; i ++ )
			{
				var refUp = sutCE.Gedimai[i];

				for( var j = i+1; j < sutCE.Gedimai.Count; j++ )
				{
					var testUp = sutCE.Gedimai[j];
					
					if( testUp.GedimoId == refUp.GedimoId )
						ModelState.AddModelError($"Gedimai[{j}].GedimoId", "Gedimo dublikatas.");
				}
			}

			//form field validation passed?
			if( ModelState.IsValid )
			{
				//update 'Sutartis'
				SutartisF2Repo.UpdateSutartis(sutCE);

				//delete all old records
				SutartisF2Repo.DeleteAptarnauja(sutCE.Sutartis.Nr);
				SutartisF2Repo.DeleteTuriGedima(sutCE.Sutartis.Nr);
				// Detales lentele
				SutartisF2Repo.DeleteDetalesKiekis(sutCE.Sutartis.Nr);
				SutartisF2Repo.DeleteAtliktasDarbas(sutCE.Sutartis.Nr);

				//create new records
				foreach (var mech in sutCE.PriklausantysMechanikai)
					SutartisF2Repo.InsertAptarnauja(sutCE.Sutartis.Nr, mech.Mechanikas);

				foreach (var ged in sutCE.Gedimai)
					SutartisF2Repo.InsertTuriGedima(sutCE.Sutartis.Nr, ged.GedimoId);

				foreach( var upVm in sutCE.AtliktiDarbai )
				{
					int AtliktasDarbasId = SutartisF2Repo.InsertAtliktasDarbas(sutCE.Sutartis.Nr, upVm);
					SutartisF2Repo.InsertDetalesKiekis(AtliktasDarbasId, upVm);
				}

				//save success, go back to the entity list
				return RedirectToAction("Index");
			}
			//form field validation failed, go back to the form
			else
			{
				PopulateLists(sutCE);
				return View(sutCE);
			}
		}

		//should not reach here
		throw new Exception("Should not reach here.");
	}

	/// <summary>
	/// This is invoked when deletion form is first opened in browser.
	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view.</returns>
	[HttpGet]
	public ActionResult Delete(int id)
	{
		var sutCE = SutartisF2Repo.FindSutartisCE(id);
		return View(sutCE);
	}

	/// <summary>
	/// This is invoked when deletion is confirmed in deletion form
	/// </summary>
	/// <param name="id">ID of the entity to delete.</param>
	/// <returns>Deletion form view on error, redirects to Index on success.</returns>
	[HttpPost]
	public ActionResult DeleteConfirm(int id)
	{
		//load 'Sutartis'
		var sutCE = SutartisF2Repo.FindSutartisCE(id);

		//'Sutartis' is in the state where deletion is permitted?
		if( sutCE.Sutartis.Busena == "priimtas laukia eileje" || sutCE.Sutartis.Busena == "atsiskaityta" )
		{
			//delete the entity
			SutartisF2Repo.DeleteAptarnauja(sutCE.Sutartis.Nr);
			SutartisF2Repo.DeleteTuriGedima(sutCE.Sutartis.Nr);
			// Detales lentele
			SutartisF2Repo.DeleteDetalesKiekis(sutCE.Sutartis.Nr);
			SutartisF2Repo.DeleteAtliktasDarbas(sutCE.Sutartis.Nr);

			SutartisF2Repo.DeleteSutartis(id);

			//redired to list form
			return RedirectToAction("Index");
		}
		//'Sutartis' is in state where deletion is not permitted
		else
		{
			//enable explanatory message and show delete form
			ViewData["deletionNotPermitted"] = true;
			return View("Delete", sutCE);
		}
	}

	/// <summary>
	/// Populates select lists used to render drop down controls.
	/// </summary>
	/// <param name="sutCE">'Sutartis' view model to append to.</param>
	private void PopulateLists(SutartisCE sutCE)
	{
		// load entities for the select lists
		// SQL ENUM equivalents.
		var ListBusenos = new List<string>{
			"remontuojamas",
			"suremontuotas",
			"priimtas laukia eileje",
			"atsaukta",
			"atsiskaityta"
		};
		// From SQL tables
		var automobiliai = AutomobilisRepo.ListAutomobilis();
		var darbuotojai = DarbuotojasRepo.List();
		var mechanikai = MechanikasRepo.List();
		var klientai = KlientasRepo.List();
		var gedimai = GedimasRepo.List();
		var detales = DetaleRepo.List();

		// build select lists
		sutCE.Lists.Automobiliai =
			automobiliai
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = it.Vin,
							Text = String.Format($"{it.ValstybinisNr[0]}{it.ValstybinisNr[1]}{it.ValstybinisNr[2]}:{it.ValstybinisNr[3]}{it.ValstybinisNr[4]}{it.ValstybinisNr[5]} - {it.Gamintojas} {it.Modelis} - {it.Vin}"),
						};
				})
				.ToList();

		sutCE.Lists.Busenos = ListBusenos
			.Select(e => new SelectListItem
			{
				Value = Convert.ToString(e),
				Text = e.ToString()
			}).ToList();

		sutCE.Lists.Darbuotojai =
			darbuotojai
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = it.Tabelis.ToString(),
							Text = $"{it.Vardas} {it.Pavarde}"
						};
				})
				.ToList();
		
		sutCE.Lists.Mechanikai =
			mechanikai
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = it.Tabelis.ToString(),
							Text = $"{it.Vardas} {it.Pavarde}"
						};
				})
				.ToList();

		sutCE.Lists.Klientai =
			klientai
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = it.AsmensKodas,
							Text = $"{it.Vardas} {it.Pavarde}"
						};
				})
				.ToList();

		sutCE.Lists.Gedimai =
			gedimai
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = it.Id.ToString(),
							Text = $"{it.EnumTipas} - {it.Pavadinimas}"
						};
				})
				.ToList();

		sutCE.Lists.Detales =
			detales
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = it.Id.ToString(),
							Text = $"{it.Pavadinimas} - {it.Kaina}€"
						};
				})
				.ToList();

		/*sutCE.Lists.Vietos =
			aiksteles
				.Select(it =>
				{
					return
						new SelectListItem
						{
							Value = Convert.ToString(it.Id),
							Text = it.Pavadinimas
						};
				})
				.ToList();*/

		//build select list for 'UzsakytosPaslaugos'
		/*{
			//initialize the destination list
			sutCE.Lists.Paslaugos = new List<SelectListItem>();

			//load 'Paslaugos' to use for item groups
			var paslaugos = PaslaugaRepo.List();

			//create select list items from 'PaslauguKainos' related to each 'Paslaugos'
			foreach( var paslauga in paslaugos )
			{
				//create list item group for current 'Paslaugos' entity
				var itemGrp = new SelectListGroup() { Name = paslauga.Pavadinimas };

				//load related 'PaslauguKaina' entities
				var kainos = PaslaugosKainaRepo.LoadForPaslauga(paslauga.Id);

				//build list items for the group
				foreach( var kaina in kainos )
				{
					var sle =
						new SelectListItem {
							Value =
								//we use JSON here to make serialization/deserializaton of composite key more convenient
								JsonConvert.SerializeObject(new {
									FkPaslauga = paslauga.Id,
									GaliojaNuo = kaina.GaliojaNuo
								}),
							Text = $"{paslauga.Pavadinimas} {kaina.Kaina} EUR ({kaina.GaliojaNuo.ToString("yyyy-MM-dd")})",
							Group = itemGrp
						};
					sutCE.Lists.Paslaugos.Add(sle);
				}
			}
		}*/
	}
}
