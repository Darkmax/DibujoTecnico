using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DB : MonoBehaviour
{

    public string db_name = "dibujo_tecnico.db";
    public SQLiteDB db = null;
    public User user;
    private string filename;

    void Awake()
    {
        db = new SQLiteDB();
        if (Application.systemLanguage == SystemLanguage.Spanish)
            Localization.language = "es";
        else
            Localization.language = "en";

        DontDestroyOnLoad(gameObject);

        filename = Application.persistentDataPath + "/" + db_name;
#if UNITY_EDITOR
        Debug.Log(Application.systemLanguage);
        Debug.Log(filename);
        Language language = new Language(1, "Español", "es");
        user = new User(1, "Hector", language);
        Localization.language = "es";
#endif
    }

    public bool checkLogin(string username, string password, bool create_user)
    {
        string query = "SELECT count(s.idUser) as count, s.idUser, s.name, l.idLanguage, l.name as language, l.code FROM Users s JOIN Languages l ON s.idLanguage = l.idLanguage WHERE s.user = ? and s.password = ?";
        bool exist_user = false;
        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(username);
                qr.Bind(password);
                qr.Step();
                if (qr.GetInteger("count") > 0)
                {
                    exist_user = true;
                    if (create_user)
                    {
                        int id_user = qr.GetInteger("idUser");
                        string name = qr.GetString("name");
                        int idLanguage = qr.GetInteger("idLanguage");
                        string language = qr.GetString("language");
                        string code = qr.GetString("code");
                        Language lan = new Language(idLanguage, language, code);
                        user = new User(id_user, name, lan);
                        Localization.language = lan.code;
                    }
                }
                qr.Release();
            }
            catch (Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.LogError(e.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            db.Close();
        }
        finally
        {
            db.Close();
        }
        return exist_user;
    }

    public List<Chapter> getChapters()
    {
        string query = @"SELECT idChapter, number, name
                        FROM Chapters WHERE idLanguage = ?";
        List<Chapter> listChapters = new List<Chapter>();

        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(user.language.idLanguage);
                while(qr.Step())
                {
                    int idChapter = qr.GetInteger("idChapter");
                    int number = qr.GetInteger("number");
                    string name = qr.GetString("name");
                    Chapter chapter = new Chapter(idChapter, number, name);
                    listChapters.Add(chapter);
                }
                qr.Release();
            }
            catch (Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.LogError(e.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            db.Close();
        }
        finally
        {
            db.Close();
        }

        return listChapters;
    }

    public List<Subject> getSubjectsFromChapter (int idChapter)
    {
        string query = @"SELECT idSubject, number, name, color, sprite, file
                        FROM Subjects
                        WHERE idChapter = ? AND idLanguage = ?";
        List<Subject> subjects = new List<Subject>();

        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(idChapter);
                qr.Bind(user.language.idLanguage);
                while (qr.Step())
                {
                    int idSubject = qr.GetInteger("idSubject");
                    int number = qr.GetInteger("number");
                    string name = qr.GetString("name");
                    string sprite = qr.GetString("sprite");
                    Color color = ParseColor(qr.GetString("color"));
                    string file = qr.GetString("file");
                    Subject subject = new Subject(idSubject, number, name, color, sprite, file);
                    subjects.Add(subject);
                }
                qr.Release();
            }
            catch (Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.LogError(e.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            db.Close();
        }
        finally
        {
            db.Close();
        }

        return subjects;
    }

    public string getFolderSubject(int idSubject)
    {
        string query = "SELECT idChapter FROM Subjects WHERE idLanguage = ? AND idSubject = ?";
        string result = "";
        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(user.language.idLanguage);
                qr.Bind(idSubject);
                if(qr.Step())
                {
                    int idChapter = qr.GetInteger("idChapter");
                    result = idChapter.ToString() + "." + idSubject.ToString();
                }
            }
            catch(Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.Log(e.ToString());
            }
        }catch(Exception e)
        {
            Debug.Log(e.ToString());
            db.Close();
        }
        finally
        {
            db.Close();
        }
        return result;
    }

    public void toggleFavorite(int idChapter, int idSubject, int numExercise, bool add)
    {
        string query_insert = "INSERT INTO Favorites(idUser, idChapter, idSubject, numExercise) VALUES(?, ?, ?, ?)";
        string query_delete = "DELETE FROM Favorites WHERE idUser = ? AND idChapter = ? AND idSubject = ? AND numExercise = ?";

        if(add)
        {
            //Add the subject and exercise to favorites
            try
            {
                db.Open(filename);
                SQLiteQuery qr = null;
                try
                {
                    qr = new SQLiteQuery(db, query_insert);
                    qr.Bind(user.idUser);
                    qr.Bind(idChapter);
                    qr.Bind(idSubject);
                    qr.Bind(numExercise);
                    qr.Step();
                }
                catch (Exception e)
                {
                    if (qr != null)
                        qr.Release();
                    Debug.Log(e.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                db.Close();
            }
            finally
            {
                db.Close();
            }
        }
        else
        {
            //Remove the subject and exercise of favorites
            try
            {
                db.Open(filename);
                SQLiteQuery qr = null;
                try
                {
                    qr = new SQLiteQuery(db, query_delete);
                    qr.Bind(user.idUser);
                    qr.Bind(idChapter);
                    qr.Bind(idSubject);
                    qr.Bind(numExercise);
                    qr.Step();
                }
                catch (Exception e)
                {
                    if (qr != null)
                        qr.Release();
                    Debug.Log(e.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                db.Close();
            }
            finally
            {
                db.Close();
            }
        }
    }

    public bool isFavoritedExercise(int idSubject, int numExercise)
    {
        string query = "SELECT count(idFavorite) as favorited FROM Favorites WHERE idSubject = ? AND numExercise = ?";
        bool value = false;

        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(idSubject);
                qr.Bind(numExercise);
                if(qr.Step())
                {
                    int favorited = qr.GetInteger("favorited");
                    if (favorited == 1) value = true;
                }
                qr.Release();
            }
            catch(Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.LogError(e.ToString());
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.ToString());
            db.Close();
        }
        finally
        {
            db.Close();
        }

        return value;
    }

//    public List<Favorite> getFavorites()
//    {
//        string query = @"SELECT c.number, c.name, s.type, e.idExercise, e.number as numberExcercise
//                        FROM Favorites f
//                        JOIN Exercises e ON f.idExercise = e.idExercise
//                        JOIN Subjects s ON e.idSubject = s.idSubject
//                        JOIN Chapters c ON c.idChapter = s.idChapter
//                        WHERE e.idUser = ?
//                        ORDER BY c.number ASC";

//        List<Favorite> listFavorites = new List<Favorite>();

//        try
//        {
//            db.Open(filename);
//            SQLiteQuery qr = null;
//            try
//            {
//                qr = new SQLiteQuery(db, query);
//                qr.Bind(user.idUser);
//                while(qr.Step())
//                {
//                    int numberChapter = qr.GetInteger("number");
//                    string name = qr.GetString("name");
//                    string type = qr.GetString("type");
//                    int idExercise = qr.GetInteger("idExercise");
//                    int numberExercise = qr.GetInteger("numberExercise");
//                    Favorite favorite = new Favorite(numberChapter, name, type, idExercise, numberExercise);
//                    listFavorites.Add(favorite);
//                }
//                qr.Release();
//            }
//            catch(Exception e)
//            {
//                if (qr != null)
//                    qr.Release();
//                Debug.LogError(e.ToString());
//            }
//        }
//        catch(Exception e)
//        {
//            Debug.LogError(e.ToString());
//            db.Close();
//        }
//        finally
//        {
//            db.Close();
//        }
        
//        return listFavorites;
//    }

    public List<FVChapter> getFavorites()
    {
//        string query_getChapters = @"SELECT f.idFavorite, f.idChapter, c.number, c.name, f.idSubject, f.numExercise
//                        FROM Favorites f JOIN Chapters c ON f.idChapter = c.idChapter
//                        WHERE f.idUser = ? AND c.idLanguage = ?";

        string query_getChapters = @"SELECT DISTINCT f.idChapter, c.number, c.name
                                    FROM Favorites f JOIN Chapters c ON f.idChapter = c.idChapter
                                    WHERE f.idUser = ? AND c.idLanguage = ?";

        string query_getSubjects = @"SELECT DISTINCT f.idSubject, s.number, s.name, s.sprite, s.color, s.file
                          FROM Favorites f JOIN Subjects s ON f.idSubject = s.idSubject
                          WHERE f.idChapter = ? AND s.idLanguage = ?";

        string query_getExercises = @"SELECT *
                                      FROM
                                      WHERE ";
        List<FVChapter> listChapter = new List<FVChapter>();

        try
        {
            db.Open(filename);
            SQLiteQuery qr_chapter = null;
            SQLiteQuery qr_exercises = null;
            try
            {
                //Get chapters from favorite folder
                qr_chapter = new SQLiteQuery(db, query_getChapters);
                qr_chapter.Bind(user.idUser);
                qr_chapter.Bind(user.language.idLanguage);
                while (qr_chapter.Step())
                {
                    FVChapter fvChapter = new FVChapter();

                    int idChapter = qr_chapter.GetInteger("idChapter");
                    int cnumber = qr_chapter.GetInteger("number");
                    string cname = qr_chapter.GetString("name");
                    Chapter chapter = new Chapter(idChapter, cnumber, cname);
                    fvChapter.chapter = chapter;

                    //Get Subjects of the chapter
                    List<FVSubject> listSubjects = new List<FVSubject>();
                    SQLiteQuery qr_subject = new SQLiteQuery(db, query_getSubjects);
                    qr_subject.Bind(idChapter);
                    qr_subject.Bind(user.language.idLanguage);
                    while(qr_subject.Step())
                    {
                        int idSubject = qr_subject.GetInteger("idSubject");
                        int snumber = qr_subject.GetInteger("number");
                        string sname = qr_subject.GetString("name");
                        string sprite = qr_subject.GetString("sprite");
                        Color color = ParseColor(qr_subject.GetString("color"));
                        string file = qr_subject.GetString("file");
                        listSubjects.Add(new FVSubject(idSubject, snumber, sname, color, sprite, file));
                    }
                    fvChapter.subjects = listSubjects;
                    listChapter.Add(fvChapter);
                }
                    //FVChapter fvchapter = new FVChapter();
                    //int idFavorite = qr.GetInteger("idFavorite");
                    //int idChapter = qr.GetInteger("idChapter");
                    
                    //int cnumber = qr.GetInteger("number");
                    //string cname = qr.GetString("name");
                    //int idSubject = qr.GetInteger("idSubject");
                    //int numExercise = qr.GetInteger("numExercise");
                    //fvchapter.chapter = new Chapter(idChapter, cnumber, cname);

                    ////Getting the subjects
                    //List<Subject> listSubjects = new List<Subject>();
                    //int snumber;
                    //Color scolor;
                    //string sname, ssprite, sfile;
                    //SQLiteQuery qr_subjects = new SQLiteQuery(db, query_getSubjects);
                    //qr_subjects.Bind(idSubject);
                    //qr_subjects.Bind(user.language.idLanguage);
                    //while (qr_subjects.Step())
                    //{
                    //    snumber = qr_subjects.GetInteger("number");
                    //    sname = qr_subjects.GetString("name");
                    //    ssprite = qr_subjects.GetString("sprite");
                    //    sfile = qr_subjects.GetString("file");
                    //    scolor = ParseColor(qr_subjects.GetString("color"));
                    //    Subject subject = new Subject(idSubject, snumber, sname, scolor, ssprite, sfile);
                    //    listSubjects.Add(subject);
                    //}
                //    fvchapter.subjects = listSubjects;

                //    //Getting the exercises
                //    List<int> listExercises = new List<int>();

                //    fvchapter.exercises = listExercises;
                //}
                //qr.Release();
            }
            catch(Exception e)
            {
                if (qr_chapter != null)
                    qr_chapter.Release();
                Debug.Log(e.ToString());
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            db.Close();
        }

        return listChapter;
    }

    public List<FVSubject> getFavoriteSubjects(int idChapter)
    {
        string query = @"SELECT DISTINCT s.idSubject, s.sprite, s.color, s.file, s.number, s.name
                        FROM Favorites f JOIN Subjects s ON f.idSubject = s.idSubject
                        WHERE f.idChapter = ? AND f.idUser = ?";

        List<FVSubject> listSubjects = new List<FVSubject>();
        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(idChapter);
                qr.Bind(user.idUser);
                while (qr.Step())
                {
                    int idSubject = qr.GetInteger("idSubject");
                    int snumber = qr.GetInteger("number");
                    string sname = qr.GetString("name");
                    string sprite = qr.GetString("sprite");
                    Color color = ParseColor(qr.GetString("color"));
                    string file = qr.GetString("file");
                    FVSubject fs = new FVSubject(idSubject, snumber, sname, color, sprite, file);
                    listSubjects.Add(fs);
                }
                qr.Release();
            }catch(Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.Log(e.ToString());
            }
        }catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            db.Close();
        }

        return listSubjects;
    }

    public List<FavoriteExercise> getFavoriteExercise(int idSubject)
    {
        string query = @"SELECT e.idExercise, e.number
                        FROM Favorites f JOIN Exercises e ON f.idExercise = e.idExercise
                        WHERE f.idSubject = ? AND f.idUser = ?";

        List<FavoriteExercise> listExercises = new List<FavoriteExercise>();
        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(idSubject);
                qr.Bind(user.idUser);
                while (qr.Step())
                {
                    int idExercise = qr.GetInteger("idExercise");
                    int number = qr.GetInteger("number");
                    FavoriteExercise fe = new FavoriteExercise(idExercise, number);
                    listExercises.Add(fe);
                }
                qr.Release();
            }
            catch (Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.Log(e.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            db.Close();
        }
        return listExercises;
    }

    /// <summary>
    /// Add a record that the user has completed an exercise of a subject
    /// </summary>
    /// <param name="idSubject"></param>
    /// <param name="numExercise"></param>
    public void addCompletedExercise(int idSubject, int numExercise)
    {
        //Check if there is no register of the exercise before
        if (!checkIfUserCompletedExercise(idSubject, numExercise))
        {
            string query = "INSERT INTO Exercises_Completed (idSubject, idUser, numExercise) VALUES(?, ?, ?)";

            try
            {
                db.Open(filename);
                SQLiteQuery qr = null;
                try
                {
                    qr = new SQLiteQuery(db, query);
                    qr.Bind(idSubject);
                    qr.Bind(user.idUser);
                    qr.Bind(numExercise);
                    qr.Step(); //Insert new register
                    qr.Release();
                }
                catch (Exception e)
                {
                    if (qr != null)
                        qr.Release();
                    Debug.Log(e.ToString());
                }
            }
            catch (Exception e) { Debug.Log(e.ToString()); }
            finally { db.Close(); }
        }
    }

    /// <summary>
    /// Get true or false if user has completed an exercise of a subject
    /// </summary>
    /// <param name="idSubject"></param>
    /// <param name="numExercise"></param>
    /// <returns></returns>
    public bool checkIfUserCompletedExercise(int idSubject, int numExercise)
    {
        string query = @"SELECT COUNT(idExerciseCompleted) as num 
                        FROM Exercises_Completed
                        WHERE idSubject = ? AND idUser = ? AND numExercise = ?";
        bool result = false;

        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(idSubject);
                qr.Bind(user.idUser);
                qr.Bind(numExercise);
                qr.Step(); //Process query
                //Check if is already a register of the exercise
                if (qr.GetInteger("num") > 0)
                    result = true;

                qr.Release();
            }
            catch (Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.Log(e.ToString());
            }
        }
        catch (Exception e) { Debug.Log(e.ToString()); }
        finally { db.Close(); }

        return result;
    }

    /// <summary>
    /// Get a list of exercises completed of a subject
    /// </summary>
    /// <param name="idSubject"></param>
    /// <returns></returns>
    public List<int> getCompletedExercises(int idSubject)
    {
        string query = @"SELECT numExercise
                        FROM Exercises_Completed
                        WHERE idUser = ? AND idSubject = ?
                        ORDER BY numExercise ASC";

        List<int> completedList = new List<int>();

        try
        {
            db.Open(filename);
            SQLiteQuery qr = null;
            try
            {
                qr = new SQLiteQuery(db, query);
                qr.Bind(user.idUser);
                qr.Bind(idSubject);
                while (qr.Step())
                {
                    int numExercise = qr.GetInteger("numExercise");
                    completedList.Add(numExercise);
                }
                qr.Release();
            }
            catch (Exception e)
            {
                if (qr != null)
                    qr.Release();
                Debug.Log(e.ToString());
            }
        }
        catch (Exception e) { Debug.Log(e.ToString()); }
        finally { db.Close(); }

        return completedList;
    }

    private Color ParseColor(string color)
    {
        string[] splitColor  = color.Split(',');
        Color output = new Color(float.Parse(splitColor[0])/255f, float.Parse(splitColor[1])/255f, float.Parse(splitColor[2])/255f, float.Parse(splitColor[3])/255f);
        return output;
    }
}
