using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PoliziaMunicipale.Models;
using System.Diagnostics;

namespace PoliziaMunicipale.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            List<Verbale> verbali = [];
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand("select * FROM Verbale as v  join Violazione as vio on vio.IDViolazione = v.IDViolazione", DB.conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var violazione = new Verbale()
                        {
                            IDVerbale = (int)reader["IDVerbale"],
                            DataViolazione = (DateTime)reader["DataViolazione"],
                            IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                            Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                            DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                            Importo = (decimal)reader["Importo"],
                            DecurtamentoPunti = (int)reader["DecurtamentoPunti"],
                            IDAnagrafica = (int)reader["IDAnagrafica"],
                            Descrizione = reader["Descrizione"].ToString()
                        };

                        verbali.Add(violazione);

                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {
                DB.conn.Close();
            }

            return View(verbali);
        }

        [HttpGet]
        public IActionResult Anagrafica([FromRoute] int? id)
        {
            var utente = new Anagrafica();

            if (id.HasValue)
            {
                try
                {
                    DB.conn.Open();
                    var cmd = new SqlCommand(@"SELECT * FROM Anagrafica WHERE IDAnagrafica = @id", DB.conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Anagrafica anagrafica = new Anagrafica
                        {
                            IDAnagrafica = Convert.ToInt32(reader["IDAnagrafica"]),
                            Cognome = Convert.ToString(reader["Cognome"]),
                            Nome = Convert.ToString(reader["Nome"]),
                            Indirizzo = Convert.ToString(reader["Indirizzo"]),
                            Citta = Convert.ToString(reader["Citta"]),
                            CAP = Convert.ToString(reader["CAP"]),
                            Cod_Fisc = Convert.ToString(reader["Cod_Fisc"])
                        };

                        return View(anagrafica);
                    }
                    else
                    {
                        return RedirectToAction("Index"); // Se l'ID non esiste, reindirizza alla pagina principale
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message); // Gestisci l'errore visualizzando un messaggio di errore
                }
                finally
                {
                    DB.conn.Close();
                }
            }
            else
            {
                return View("Error");
            }


        }

        [HttpGet]
        public IActionResult Add()
        {
            List<Anagrafica> utenti = new List<Anagrafica>();
            List<Violazione> violazioni = new List<Violazione>();

            try
            {
                DB.conn.Open();

                // Carica le violazioni
                var cmdViolazioni = new SqlCommand("SELECT * FROM Violazione", DB.conn);
                var readerViolazioni = cmdViolazioni.ExecuteReader();

                if (readerViolazioni.HasRows)
                {
                    while (readerViolazioni.Read())
                    {
                        var violazione = new Violazione()
                        {
                            IDViolazione = (int)readerViolazioni["IDViolazione"],
                            Descrizione = readerViolazioni["Descrizione"].ToString()
                        };

                        violazioni.Add(violazione);
                    }

                    readerViolazioni.Close();
                }

                // Carica gli utenti
                var cmdUtenti = new SqlCommand("SELECT * FROM Anagrafica", DB.conn);
                var readerUtenti = cmdUtenti.ExecuteReader();

                if (readerUtenti.HasRows)
                {
                    while (readerUtenti.Read())
                    {
                        var utente = new Anagrafica()
                        {
                            IDAnagrafica = (int)readerUtenti["IDAnagrafica"],
                            Nome = readerUtenti["Nome"].ToString(),
                            Cognome = readerUtenti["Cognome"].ToString(),
                            Indirizzo = readerUtenti["Indirizzo"].ToString(),
                            Citta = readerUtenti["Citta"].ToString(),
                            CAP = readerUtenti["CAP"].ToString(),
                            Cod_Fisc = readerUtenti["Cod_Fisc"].ToString()
                        };

                        utenti.Add(utente);
                    }

                    readerUtenti.Close();
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            finally
            {
                DB.conn.Close();
            }

            ViewBag.Violazioni = violazioni;
            ViewBag.Utenti = utenti;

            return View();
        }
        [HttpPost]
        public IActionResult Add(Verbale verbale)
        {
            var error = true;
            try
            {
                DB.conn.Open();
                var cmd = new SqlCommand(@"INSERT INTO Verbale 
                                           (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, IDAnagrafica, IDViolazione)
                                           VALUES (@dataViolazione, @indirizzoViolazione, @nominativo_agente, @dataTrascrizioneVerbale, @importo, @decurtamentoPunti, @idAnagrafica, @idViolazione)", DB.conn);
                cmd.Parameters.AddWithValue("@dataViolazione", verbale.DataViolazione);
                cmd.Parameters.AddWithValue("@indirizzoViolazione", verbale.IndirizzoViolazione);
                cmd.Parameters.AddWithValue("@nominativo_agente", verbale.Nominativo_Agente);
                cmd.Parameters.AddWithValue("@dataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                cmd.Parameters.AddWithValue("@importo", verbale.Importo);
                cmd.Parameters.AddWithValue("@decurtamentoPunti", verbale.DecurtamentoPunti);
                cmd.Parameters.AddWithValue("@idAnagrafica", verbale.IDAnagrafica);
                cmd.Parameters.AddWithValue("@idViolazione", verbale.IDViolazione);

                var nRows = cmd.ExecuteNonQuery();
                if (nRows > 0)
                {
                    error = false;
                }

            }
            catch (Exception ex)
            {
                
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }

            if (!error)
            {
                TempData["MessageSuccess"] = $"La violazione dell'agente {verbale.Nominativo_Agente} dal totale di  {verbale.Importo} � andata a buon fine";
            }
            else
            {
                TempData["MessageError"] = "ci sono stati problemi con l'inserimento dei dati nel DataBase";
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddAnagrafica()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAnagrafica(Anagrafica utente)
        {
            var error = true;
            int lastInsertedId = 0;

            try
            {

                DB.conn.Open();

                var cmd = new SqlCommand(@"INSERT INTO Anagrafica 
                                                     (Cognome, Nome, Indirizzo, Citta, CAP, Cod_Fisc)
                                                     VALUES (@cognome, @nome, @indirizzo, @citta, @cap, @cod_fisc);
                                                     SELECT SCOPE_IDENTITY()", DB.conn);

                cmd.Parameters.AddWithValue("@cognome", utente.Cognome);
                cmd.Parameters.AddWithValue("@nome", utente.Nome);
                cmd.Parameters.AddWithValue("@indirizzo", utente.Indirizzo);
                cmd.Parameters.AddWithValue("@citta", utente.Citta);
                cmd.Parameters.AddWithValue("@cap", utente.CAP);
                cmd.Parameters.AddWithValue("@cod_fisc", utente.Cod_Fisc);


                var identity = cmd.ExecuteScalar();

                if (identity != null)
                {
                    lastInsertedId = Convert.ToInt32(identity);
                    error = false;
                }


            }
            catch (Exception ex)
            {

                return View("Error");
            }
            finally
            {
                DB.conn.Close();
            }


            if (!error)
            {
                TempData["MessageSuccess"] = $"L'utente {utente.Nome} {utente.Cognome} � stato aggiunto in anagrafica.";
                return RedirectToAction("Anagrafica", new { id = lastInsertedId });
            }
            else
            {
                TempData["MessageError"] = $"Errore durante il caricamento nella base dati.";
                return RedirectToAction("Index");
            }


        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Anagrafica anagrafica = null;

            try
            {
                DB.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Anagrafica WHERE IDAnagrafica = @id", DB.conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                if (reader.Read()) 
                {
                    anagrafica = new Anagrafica
                    {
                        IDAnagrafica = (int)reader["IDAnagrafica"],
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Indirizzo = reader["Indirizzo"].ToString(),
                        Citta = reader["Citta"].ToString(),
                        CAP = reader["CAP"].ToString(),
                        Cod_Fisc = reader["Cod_Fisc"].ToString()
                    };

                     
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");
            }
            finally
            {
                DB.conn.Close();
            }



            return View(anagrafica);
        }

        [HttpPost]

        public IActionResult Edit(Anagrafica utente)
        {
            
            try
            {
                DB.conn.Open();

                var cmd = new SqlCommand(@"UPDATE Anagrafica 
                           SET Cognome = @cognome, 
                               Nome = @nome, 
                               Indirizzo = @indirizzo, 
                               Citta = @citta, 
                               CAP = @cap, 
                               Cod_Fisc = @cod_fisc
                           WHERE IDAnagrafica = @id", DB.conn);

                cmd.Parameters.AddWithValue("@cognome", utente.Cognome);
                cmd.Parameters.AddWithValue("@nome", utente.Nome);
                cmd.Parameters.AddWithValue("@indirizzo", utente.Indirizzo);
                cmd.Parameters.AddWithValue("@citta", utente.Citta);
                cmd.Parameters.AddWithValue("@cap", utente.CAP);
                cmd.Parameters.AddWithValue("@cod_fisc", utente.Cod_Fisc);
                cmd.Parameters.AddWithValue("@id", utente.IDAnagrafica);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                DB.conn.Close();
            }

            return RedirectToAction("Anagrafica", new {id = utente.IDAnagrafica});

    }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


    
}
