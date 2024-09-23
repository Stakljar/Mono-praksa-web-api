import Button from "./Button";

export default function CatTable({cats, editCat, deleteCat }) {
    return (
        <table id="catTable">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Age</th>
                    <th>Color</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                {cats?.length > 0 ? (
                    cats.map((cat, index) => (
                        <tr key={index}>
                            <td>{cat.id}</td>
                            <td>{cat.name}</td>
                            <td>{cat.age}</td>
                            <td>{cat.color}</td>
                            <td><Button className="edit-btn" onClick={() => editCat(cat)} text="Edit" /></td>
                            <td><Button className="delete-btn" onClick={() => deleteCat(cat)} text="Delete" /></td>
                        </tr>
                    ))
                ) : (
                    <tr>
                        <td>No cats available</td>
                    </tr>
                )}
            </tbody>
        </table>
    )
}
