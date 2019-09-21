;;****************
;;* DEFFUNCTIONS *
;;****************

(deffunction ask-question (?question $?allowed-values)
   (printout t ?question)
   (bind ?answer (read))
   (if (lexemep ?answer) 
       then (bind ?answer (lowcase ?answer)))
   (while (not (member$ ?answer ?allowed-values)) do
      (printout t ?question)
      (bind ?answer (read))
      (if (lexemep ?answer) 
          then (bind ?answer (lowcase ?answer))))
   ?answer)

(deffunction yes-or-no-p (?question)
   (bind ?response (ask-question ?question yes no y n))
   (if (or (eq ?response yes) (eq ?response y))
       then yes 
       else no))

;;;***************
;;;* QUERY RULES *
;;;***************

(defrule rabot_culer ""
   (not (culer_rab ?))
   (not (repair ?))
   =>
   (assert (culer_rab (yes-or-no-p "Kuler chistii (yes/no)? "))))

(defrule vent_krutit ""
   (culer_rab yes)
   (not (repair ?))
   =>
   (assert (vent_krutit (yes-or-no-p "Ventilyator krutitsya (yes/no)? "))))

(defrule comp_virus ""
   (culer_rab yes)
   (vent_krutit yes)
   (not (repair ?))
   =>
   (assert (comp_virus (yes-or-no-p "Computer zaragen virusami (yes/no)? "))))

(defrule cboi_drivers ""
   (culer_rab yes)
   (vent_krutit yes)
   (comp_virus no)
   (not (repair ?))
   =>
   (assert (cboi_drivers (yes-or-no-p "Est cboi draiverov (yes/no)? "))))

(defrule blok_pit ""
   (culer_rab yes)
   (vent_krutit yes)
   (comp_virus no)
   (cboi_drivers no)
   (not (repair ?))
   =>
   (assert (blok_pit (yes-or-no-p "Blok pitaniya holodniy (yes/no)? "))))

(defrule ogol_pr ""
   (culer_rab yes)
   (vent_krutit yes)
   (comp_virus no)
   (cboi_drivers no)
   (blok_pit yes)
   (not (repair ?))
   =>
   (assert (ogol_pr (yes-or-no-p "Provoda ogoleny (yes/no)? "))))

(defrule ploh_cab ""
   (ogol_pr yes)
   (not (repair ?))
   =>
   (assert (ploh_cab (yes-or-no-p "Cabel manager plohoi (yes/no)? "))))

(defrule culer_CP_slom ""
   (or(culer_rab no)
   (vent_krutit no))
   (not (repair ?))
   =>
   (assert (culer_CP_slom yes)))

(defrule sboi_po ""
   (or(comp_virus yes)
   (cboi_drivers yes))
   (not (repair ?))
   =>
   (assert (sboi_po yes)))

(defrule prov_slom ""
   (ogol_pr yes)
   (ploh_cab yes)
   (not (repair ?))
   =>
   (assert (prov_slom yes)))

(defrule ne_vid_gd ""
   (or(sboi_po yes)
   (prov_slom yes))
   (not (repair ?))
   =>
   (assert (ne_vid_gd yes)))

(defrule st_zvuk ""
   (ne_vid_gd yes)
   (not (repair ?))
   =>
   (assert (st_zvuk (yes-or-no-p "Ctrannye zvuki prisutstvuyt (yes/no)? "))))

(defrule prob_pit ""
   (prov_slom yes)
   (blok_pit yes)
   (not (repair ?))
   =>
   (assert (repair "Problems s pitaniem.")))

(defrule sboi_gd ""
   (st_zvuk yes)
   (ne_vid_gd yes)
   (not (repair ?))
   =>
   (assert (repair "Sboi GD.")))

(defrule sboi_proc ""
   (or(culer_CP_slom yes)
   (prov_slom yes))
   (not (repair ?))
   =>
   (assert (repair "Sboi processora.")))

(defrule neizvestno ""
   (culer_rab yes)
   (vent_krutit yes)
   (comp_virus no)
   (cboi_drivers no)
   (blok_pit no)
   (ogol_pr no)
   (ploh_cab yes)
   (not (repair ?))
   =>
   (assert (repair "Neizvestno.")))

;;;********************************
;;;* STARTUP AND CONCLUSION RULES *
;;;********************************

(defrule system-banner ""
  (declare (salience 10))
  =>
  (printout t crlf crlf)
  (printout t "The Engine Diagnosis Expert System")
  (printout t crlf crlf))

(defrule print-repair ""
  (declare (salience 10))
  (repair ?item)
  =>
  (printout t crlf crlf)
  (printout t "Suggested Repair:")
  (printout t crlf crlf)
  (format t " %s%n%n%n" ?item))